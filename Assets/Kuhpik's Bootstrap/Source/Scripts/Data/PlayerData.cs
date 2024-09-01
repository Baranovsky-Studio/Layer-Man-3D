using System;
using System.Collections.Generic;
using Idle_Arcade_Components.Scripts.Components;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // Example (I use public fields for data, but u free to use properties\methods etc)
        // [BoxGroup("level")] public int level;
        // [BoxGroup("currency")] public int money;
        
        public bool Sounds = true;
        public bool Vibration = true;
        
        public int[] ResourcesCounts;
        
        public List<UpgradableItemData> UpgradableItemsData = new List<UpgradableItemData>();

        public int BossId;
        
        public int CircleSkinId;
        public int TrailId;
        public int MaterialId;
        
        public List<UnlockableItemData> TrailsData = new List<UnlockableItemData>();
        public List<UnlockableItemData> LayersData = new List<UnlockableItemData>();
        public List<UnlockableItemData> ColorsData = new List<UnlockableItemData>();

        public int LevelId = 1;
        public int GameId = 1;
    }

    [Serializable]
    public class LevelData
    {
    }
}