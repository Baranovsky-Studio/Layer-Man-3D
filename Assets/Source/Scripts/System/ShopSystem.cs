using Kuhpik;
using UnityEngine;

public class ShopSystem : GameSystem
{
    [SerializeField] private Shop _shop;
    
    public override void OnInit()
    {
        base.OnInit();
        _shop.InitializeItems(player.TrailsData.Count == 0);
    }
}
