using BaranovskyStudio;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Shop : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _close;
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _preview;
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _main;

    
    [BoxGroup("BUYING ITEMS")] [SerializeField]
    private Button _buyRandomItem;
    [BoxGroup("BUYING ITEMS")] [SerializeField]
    private int _price;
    
    [BoxGroup("BANKNOTES")] [SerializeField]
    private Button _getBanknotes;
    [BoxGroup("BANKNOTES")] [SerializeField]
    private int _banknotes = 100;
    
    
    [BoxGroup("LISTS")] [SerializeField] 
    private ShopList[] _lists;
    [BoxGroup("LISTS")] [SerializeField] 
    private ShopList _currentList;
    
    void OnEnable()
    {
        _getBanknotes.gameObject.SetActive(true);
    }

    private void Start()
    {
        _close.onClick.AddListener(OnCloseButtonClick);
        _buyRandomItem.onClick.AddListener(OnBuyRandomItemButtonClick);
        _getBanknotes.onClick.AddListener(OnGetBanknotesButtonClick);

        YandexGame.RewardVideoEvent += OnRewardedShown;

        foreach (var list in _lists)
        {
            list.OnOpen += OnOpenList;
            list.OnUnlockAll += OnUnlockAll;
        }
        
        OnOpenList(_lists[1]);
    }

    private void OnOpenList(ShopList shopList)
    {
        _currentList = shopList;
        
        _getBanknotes.gameObject.SetActive(!shopList.IsAllItemUnlocked);
        _buyRandomItem.gameObject.SetActive(!shopList.IsAllItemUnlocked);
    }

    private void OnUnlockAll()
    {
        _getBanknotes.gameObject.SetActive(false);
        _buyRandomItem.gameObject.SetActive(false);
    }

    public void InitializeItems(bool isFirst)
    {
        foreach (var list in _lists)
        {
            foreach (var item in list.Items)
            {
                if (isFirst)
                {
                    item.InitializeAtFirst();
                }
                else
                {
                    item.Initialize();
                }
            }
        }
    }

    private void OnCloseButtonClick()
    {
        _main.SetActive(true);
        gameObject.SetActive(false);
        _preview.SetActive(false);
    }

    private void OnBuyRandomItemButtonClick()
    {
        Bootstrap.Instance.GetSystem<ResourcesSystem>().TryToBuy(ResourcesSystem.ResourceType.Banknotes, _price, () =>
        {
            _currentList.BuyRandomItem();
        });
    }

    private void OnGetBanknotesButtonClick()
    {
        YandexGame.RewVideoShow(33);
        _getBanknotes.gameObject.SetActive(false);
    }

    private void OnRewardedShown(int n)
    {
        if (n == 33)
        {
            Bootstrap.Instance.GetSystem<ResourcesSystem>().AddResourceCount(ResourcesSystem.ResourceType.Banknotes, _banknotes);
            Bootstrap.Instance.SaveGame();
        }
    }
}
