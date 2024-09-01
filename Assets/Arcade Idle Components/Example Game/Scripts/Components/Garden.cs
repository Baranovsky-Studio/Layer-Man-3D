using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    public class Garden : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] 
        private Tree[] _trees;

        [BoxGroup("UPGRADABLE ITEMS")] [SerializeField]
        private UpgradableItem _growTimeUpgradableItem;
        
        [BoxGroup("SETTINGS")] [SerializeField]
        private int[] _growTime;

        private void Awake()
        {
            _growTimeUpgradableItem.OnUpgradeItem.AddListener(OnUpgradeGrowTime);
            //Sets grow time by current upgrade level
            OnUpgradeGrowTime(_growTimeUpgradableItem.UpgradeLevel); 
        }

        /// <summary>
        /// Sets grow time to all trees.
        /// </summary>
        private void OnUpgradeGrowTime(int upgradeLevel)
        {
            foreach (var tree in _trees)
            {
                tree.GrowTime = _growTime[upgradeLevel];
            }
        }
    }
}
