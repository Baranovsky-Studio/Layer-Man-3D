using System;
using Idle_Arcade_Components.Scripts.Components;
using Kuhpik;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShopItem : MonoBehaviour
{
    public enum ItemType
    {
        Trails,
        Layers,
        Colors
    }
    
    [BoxGroup("SETTINGS")] [SerializeField]
    private int _id;
    [BoxGroup("SETTINGS")] [SerializeField]
    private bool _isUnlockedByDefault;
    [BoxGroup("SETTINGS")] [SerializeField]
    private ItemType _itemType;

    [BoxGroup("OPEN AT LEVEL")] [SerializeField]
    private bool _useOpenAtLevel;
    [BoxGroup("OPEN AT LEVEL")] [SerializeField] [ShowIf("_useOpenAtLevel")]
    private int _levelId;
    [BoxGroup("OPEN AT LEVEL")] [SerializeField] [ShowIf("_useOpenAtLevel")]
    private GameObject _notification;
    [BoxGroup("OPEN AT LEVEL")] [SerializeField] [ShowIf("_useOpenAtLevel")]
    private TextMeshProUGUI _levelIdText;
    
    [BoxGroup("UNLOCK BY AD")] [SerializeField]
    private bool _useUnlockingByAd;
    [BoxGroup("UNLOCK BY AD")] [SerializeField] [ShowIf("_useUnlockingByAd")]
    private Button _unlockByAd;

    [BoxGroup("ICONS")] [SerializeField] 
    private Image _icon;
    [BoxGroup("ICONS")] [SerializeField] 
    private Sprite _locked;
    [BoxGroup("ICONS")] [SerializeField] 
    private Sprite _unlocked;
    
    [BoxGroup("NO TAIL")] [SerializeField] 
    private bool _useNoTail;
    [BoxGroup("NO TAIL")] [SerializeField] [ShowIf("_useNoTail")]
    private Sprite _noTailRus;
    
    [BoxGroup("SELECTABLE")] [SerializeField] 
    private Button _select;
    [BoxGroup("SELECTABLE")] [SerializeField] 
    private Image _selectItemImage;
    
    public bool IsUnlocked;
    public Action<ShopItem> OnUnlock;
    public Action<ShopItem> OnSelect;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _select.onClick.AddListener(SelectItem);
        
        if (_useUnlockingByAd)
        {
            _unlockByAd.onClick.AddListener(OnUnlockByAdClick);
            YandexGame.RewardVideoEvent += OnRewardedShown;
        }
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnUnlockByAdClick()
    {
        YandexGame.RewVideoShow(gameObject.GetInstanceID());
    }

    private void OnRewardedShown(int n)
    {
        if (n == gameObject.GetInstanceID())
        {
            Unlock();
        }
    }

    public void InitializeAtFirst()
    {
        switch (_itemType)
        {
            case ItemType.Trails:
                Bootstrap.Instance.PlayerData.TrailsData.Add(new UnlockableItemData {Id = _id, IsUnlocked = _isUnlockedByDefault});
                break;
            case ItemType.Layers:
                Bootstrap.Instance.PlayerData.LayersData.Add(new UnlockableItemData {Id = _id, IsUnlocked = _isUnlockedByDefault});
                break;
            case ItemType.Colors:
                Bootstrap.Instance.PlayerData.ColorsData.Add(new UnlockableItemData {Id = _id, IsUnlocked = _isUnlockedByDefault});
                break;
        }
        
        Bootstrap.Instance.SaveGame();
        
        Initialize();
    }

    public void Initialize()
    {
        var data = _itemType switch
        {
            ItemType.Trails => Bootstrap.Instance.PlayerData.TrailsData[_id],
            ItemType.Layers => Bootstrap.Instance.PlayerData.LayersData[_id],
            ItemType.Colors => Bootstrap.Instance.PlayerData.ColorsData[_id],
            _ => new UnlockableItemData()
        };

        IsUnlocked = data.IsUnlocked;

        if (_useOpenAtLevel)
        {
            if (_levelId <= Bootstrap.Instance.PlayerData.LevelId)
            {
                Unlock();
            }
        }
        
        if (IsUnlocked)
        {
            OnUnlock?.Invoke(this);
        }
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IsUnlocked)
        {
            if (_useNoTail)
            {
                _icon.sprite = YandexGame.EnvironmentData.language == "ru" ? _noTailRus : _unlocked;
            }
            else
            {
                _icon.sprite = _unlocked;
            }

            _notification.SetActive(false);
            _unlockByAd.gameObject.SetActive(false);
            UpdateItemSelection();
        }
        else
        {
            if (_useNoTail)
            {
                _icon.sprite = YandexGame.EnvironmentData.language == "ru" ? _noTailRus : _unlocked;
            }
            else
            {
                _icon.sprite = _locked;
            }
            
            if (_useOpenAtLevel)
            {
                _notification.SetActive(true);
                _levelIdText.text = _levelId.ToString();
            }

            if (_useUnlockingByAd)
            {
                _unlockByAd.gameObject.SetActive(true);
            }
        }
    }

    private void SelectItem()
    {
        if (!IsUnlocked) return;
        
        switch (_itemType)
        {
            case ItemType.Trails:
                Bootstrap.Instance.PlayerData.TrailId = _id;
                break;
            case ItemType.Layers:
                Bootstrap.Instance.PlayerData.CircleSkinId = _id;
                break;
            case ItemType.Colors:
                Bootstrap.Instance.PlayerData.MaterialId = _id;
                break;
        }
        
        _selectItemImage.enabled = true;
        OnSelect?.Invoke(this);
        Bootstrap.Instance.SaveGame();
    }

    private void UpdateItemSelection()
    {
        switch (_itemType)
        {
            case ItemType.Trails:
                if (Bootstrap.Instance.PlayerData.TrailId == _id) {
                    _selectItemImage.enabled = true;
                    OnSelect?.Invoke(this);
                }
                break;
            case ItemType.Layers:
                if (Bootstrap.Instance.PlayerData.CircleSkinId == _id) {
                    _selectItemImage.enabled = true;
                    OnSelect?.Invoke(this);
                }
                break;
            case ItemType.Colors:
                if (Bootstrap.Instance.PlayerData.MaterialId == _id) {
                    _selectItemImage.enabled = true;
                    OnSelect?.Invoke(this);
                }
                break;
        }
    }

    public void DeselectItem()
    {
        _selectItemImage.enabled = false;
    }

    public void PlaySizing()
    {
        _animator.SetTrigger("Size");
    }

    public void Unlock()
    {
        IsUnlocked = true;
        
        switch (_itemType)
        {
            case ItemType.Trails:
                Bootstrap.Instance.PlayerData.TrailsData[_id].IsUnlocked = true;
                break;
            case ItemType.Layers:
                Bootstrap.Instance.PlayerData.LayersData[_id].IsUnlocked = true;
                break;
            case ItemType.Colors:
                Bootstrap.Instance.PlayerData.ColorsData[_id].IsUnlocked = true;
                break;
        }
        
        Bootstrap.Instance.SaveGame();
        OnUnlock?.Invoke(this);
        SelectItem();
        UpdateUI();
    }
}
