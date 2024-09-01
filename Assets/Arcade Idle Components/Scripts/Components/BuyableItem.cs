using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Buyable Item", 0)]
    public class BuyableItem : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop button buy here.")]
        private ButtonBuy _buttonBuy;
        
        [BoxGroup("SETTINGS")] [SerializeField]
        private int _price;
        [BoxGroup("SETTINGS")] [SerializeField]
        private ResourcesSystem.ResourceType _paymentResource;

        [BoxGroup("EVENTS")]
        public UnityEvent OnBought;

        private void Start()
        {
            CheckForErrors();
            _buttonBuy.Button.onClick.AddListener(TryToBuyItem);
            _buttonBuy.SetText(_price.ToString());
        }
        
        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_buttonBuy == null)
                {
                    Debug.LogError("Buyable Item: button buy is null.");
                }
            }
        }

        private void TryToBuyItem()
        {
            ResourcesSystem.Instance.TryToBuy(_paymentResource, _price, OnBuyComplete);
        }

        private void OnBuyComplete()
        {
            OnBought?.Invoke();
        }
    }
}