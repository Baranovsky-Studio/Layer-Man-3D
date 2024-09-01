using System;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    [Serializable]
    public class MarketItemSettings
    {
        public int PriceLevel { get; set; }
        [SerializeField] private int[] _prices;

        public int Price => _prices[PriceLevel];
    }
}