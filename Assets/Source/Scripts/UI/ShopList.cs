using System;
using System.Collections;
using System.Collections.Generic;
using BaranovskyStudio;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopList : UIElement
{
    public ShopItem[] Items;
    public event Action<ShopList> OnOpen;
    public event Action OnUnlockAll;
    public bool IsAllItemUnlocked;
    
    private List<ShopItem> _lockedItems = new List<ShopItem>();

    private void Awake()
    {
        foreach (var item in Items)
        {
            item.OnUnlock += OnUnlockItem;
            item.OnSelect += OnSelect;
            if(item.IsUnlocked) continue;
            _lockedItems.Add(item);
        }
        IsAllItemUnlocked = _lockedItems.Count == 0;
    }

    private void OnUnlockItem(ShopItem item)
    {
        _lockedItems.Remove(item);
        
        if (_lockedItems.Count == 0)
        {
            IsAllItemUnlocked = _lockedItems.Count == 0;
            OnUnlockAll?.Invoke();
        }
    }

    private void OnSelect(ShopItem selectedItem)
    {
        foreach (var item in Items)
        {
            if (item.IsUnlocked && item.Equals(selectedItem) == false)
            {
                item.DeselectItem();
            }
        }
    }

    public void BuyRandomItem()
    {
        StartCoroutine(BuyingLoop());
    }

    private IEnumerator BuyingLoop()
    {
        for (var n = 0; n <= 10; n++)
        {
            var randomId = Random.Range(0, _lockedItems.Count);
            _lockedItems[randomId].PlaySizing();
            yield return new WaitForSeconds(0.1f);
        }
        
        var id = Random.Range(0, _lockedItems.Count);
        _lockedItems[id].PlaySizing();
        _lockedItems[id].Unlock();
    }

    protected override void OnShow()
    {
        OnOpen?.Invoke(this);
    }

    protected override void OnHide() {}
}
