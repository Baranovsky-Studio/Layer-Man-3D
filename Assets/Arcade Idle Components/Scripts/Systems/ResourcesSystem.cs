using System;
using System.Linq;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio
{
    public class ResourcesSystem : GameSystem
    {
        public enum ResourceType
        {
            Banknotes,
        }

        [Serializable]
        public struct ResourceSettings
        {
            public ResourceType Type;
            public ResourceCounter Counter;
        }

        [BoxGroup("SETTINGS")] [SerializeField] 
        private ResourceSettings[] _resources;
        [BoxGroup("SETTINGS")] [SerializeField] [OnValueChanged("OnTestModeValueChanged")]
        private bool _testMode;
        [BoxGroup("SETTINGS")] [SerializeField] [ShowIf("_testMode")]
        private int[] _testResourcesCounts;

        public static ResourcesSystem Instance;

        private void OnTestModeValueChanged()
        {
            if (_testMode)
            {
                _testResourcesCounts = new int[_resources.Length];
            }
        }

        public override void OnInit()
        {
            Instance = this;
            
            if (Bootstrap.Instance.PlayerData.ResourcesCounts == null)
            {
                Bootstrap.Instance.PlayerData.ResourcesCounts = new int[_resources.Length];
                Bootstrap.Instance.SaveGame();
            }
            else if(Bootstrap.Instance.PlayerData.ResourcesCounts.Length < _resources.Length)
            {
                Bootstrap.Instance.PlayerData.ResourcesCounts = new int[_resources.Length];
                Bootstrap.Instance.SaveGame();
            }
            
            CheckForErrors();

            foreach (var resource in _resources)
            {
                var resourceId = (int) resource.Type;

                if (_testMode)
                {
                    Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId] = _testResourcesCounts[resourceId];
                    Bootstrap.Instance.SaveGame();
                }

                resource.Counter.SetValue(Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId]);   
            }
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_resources.Length == 0)
                {
                    Debug.LogError("Resources System: resources settings is empty.");
                }
            }
        }

        public int GetResourceCount(ResourceType type)
        {
            var resourceId = (int) type;
            return Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId];
        }
        
        public void AddResourceCount(ResourceType type, int value)
        {
            var resourceId = (int) type;
            var resourceCount = Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId];
            var clampedValue = Mathf.Clamp(resourceCount + value, 0, int.MaxValue);
            
            Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId] = clampedValue;
            Bootstrap.Instance.SaveGame();

            var counter = _resources.First(m => m.Type.Equals(type)).Counter;
            counter.SetValue(clampedValue);
        }

        /// <summary>
        /// Subtracts the price from the number of banknotes if they are enough and returns it through action.
        /// </summary>
        /// <param name="type">What resource will pay for the purchase.</param>
        /// <param name="price">The value to be deducted from the total number of resource.</param>
        /// <param name="onComplete">An event that is called if there are enough resource.</param>
        public void TryToBuy(ResourceType type, int price, Action onComplete)
        {
            var resourceId = (int) type;
            if (Bootstrap.Instance.PlayerData.ResourcesCounts[resourceId] >= price)
            {
                AddResourceCount(type, -price);
                onComplete?.Invoke();
            }
        }
    }
}