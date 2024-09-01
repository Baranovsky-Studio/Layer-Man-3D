using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YG;

public class MenuUIScreen : UIScreen
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Counter _levelCounter;
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _play;
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _actions;
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _upgrades;

    [BoxGroup("SHOP")] [SerializeField] 
    private Button _openShop;
    [BoxGroup("SHOP")] [SerializeField] 
    private GameObject _shop;
    [BoxGroup("SHOP")] [SerializeField] 
    private GameObject _shopPreview;
    [BoxGroup("SHOP")] [SerializeField] 
    private GameObject _main;

    [BoxGroup("ADVERTISEMENT")] [SerializeField]
    private Button _ad;

    private void Start()
    {
        _play.onClick.AddListener(OnButtonPlayClick);
        _openShop.onClick.AddListener(OnButtonShopClick);
        _ad.onClick.AddListener(OnButtonAdClick);
    }

    public void OnCameraReady()
    {
        _actions.gameObject.SetActive(true);
    }

    public override void Open()
    {
        base.Open();
        _levelCounter.SetValue(Bootstrap.Instance.PlayerData.GameId);
        _actions.gameObject.SetActive(false);

        if (Bootstrap.Instance.PlayerData.GameId > 1)
        {
            _upgrades.SetActive(true);
            _ad.gameObject.SetActive(true);
        }
        if (Bootstrap.Instance.PlayerData.GameId > 2)
        {
            _openShop.gameObject.SetActive(true);
        }
    }

    private void OnButtonPlayClick()
    {
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
    }

    private void OnButtonShopClick()
    {
        _shop.SetActive(true);
        _shopPreview.SetActive(true);
        _main.SetActive(false);
    }

    private void OnButtonAdClick()
    {
        YandexGame.Instance._OnURL_Yandex_DefineDomain("app/265962");
        _ad.gameObject.SetActive(false);
    }
}
