using System.Collections.Generic;
using Idle_Arcade_Components.Scripts.Components;

namespace BaranovskyStudio
{
    public class PlayerData
    {
        //Here you can save all the data you need
        //For example
        //public int LevelId;
        public bool Sounds = true;
        public bool Vibration = true;
        
        public int[] ResourcesCounts;

        public int LevelId = 1;

        public List<UnlockableItemData> UnlockableItemsData = new List<UnlockableItemData>();
        public List<UpgradableItemData> UpgradableItemsData = new List<UpgradableItemData>();
    }
}