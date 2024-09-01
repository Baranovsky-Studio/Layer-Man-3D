using BaranovskyStudio;
using Kuhpik;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class UpgradesItems : GameSystem
    {
        [SerializeField] 
        private UpgradableItem _circlesCounts;
        [SerializeField] 
        private UpgradableItem _income;
        [SerializeField] 
        private Settings _settings;
        
        public override void OnInit()
        {
            base.OnInit();
            
            _circlesCounts.Initialize();
            _income.Initialize();
            
            _circlesCounts.OnUpgradeItem.AddListener(OnCirclesUpgrade);
            _income.OnUpgradeItem.AddListener(OnIncomeUpgrade);
            
            Bootstrap.Instance.GameData.StandartCircles = _settings.CirclesCounts[_circlesCounts.UpgradeLevel];
            Bootstrap.Instance.GameData.IncomeMultiplier = _settings.Incomes[_income.UpgradeLevel];
        }

        private void OnCirclesUpgrade(int upgradeLevel)
        {
            Bootstrap.Instance.GameData.StandartCircles = _settings.CirclesCounts[upgradeLevel];
            var defaultCirclesCount = _settings.CirclesCounts[_circlesCounts.UpgradeLevel];
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBackpack>().UpdateDefaultCirclesCount(defaultCirclesCount);
        }

        private void OnIncomeUpgrade(int upgradeLevel)
        {
            Bootstrap.Instance.GameData.IncomeMultiplier = _settings.Incomes[upgradeLevel];
        }
    }
}