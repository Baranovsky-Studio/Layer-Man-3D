using BaranovskyStudio;
using Kuhpik;
using NaughtyAttributes;
using Pocket_Snake;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class WinUIScreen : UIScreen
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Counter _levelCounter;
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _claimX2;
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _claim;
    [BoxGroup("LINKS")] [SerializeField] 
    private TextMeshProUGUI _rewardCount;

    private int _reward;
    
    private void Start()
    {
        _claim.onClick.AddListener(OnClaimButtonClick);
        _claimX2.onClick.AddListener(OnClaimX2ButtonClick);
        YandexGame.RewardVideoEvent += OnRewardedShown;
    }
    
    public override void Open()
    {
        base.Open();
        _claimX2.gameObject.SetActive(true);
        _levelCounter.SetValue(Bootstrap.Instance.PlayerData.GameId);

        _reward = (int) (Bootstrap.Instance.GameData.Reward * Bootstrap.Instance.GameData.IncomeMultiplier);
        _rewardCount.text = _reward.ToString();
        
        if (Bootstrap.Instance.PlayerData.LevelId + 1 < SceneManager.sceneCountInBuildSettings)
        {
            Bootstrap.Instance.PlayerData.LevelId++;
        }
        else
        {
            Bootstrap.Instance.PlayerData.LevelId = 4;
        }
        
        Bootstrap.Instance.PlayerData.GameId++;
        Bootstrap.Instance.SaveGame();
    }

    private void OnClaimButtonClick()
    {
        Bootstrap.Instance.GetSystem<LoadLevelSystem>().LoadLevel();
        Bootstrap.Instance.ChangeGameState(GameStateID.Menu);
        Bootstrap.Instance.GetSystem<ResourcesSystem>().AddResourceCount(ResourcesSystem.ResourceType.Banknotes, _reward);
        
        YandexGame.FullscreenShow();
    }

    private void OnClaimX2ButtonClick()
    {
        YandexGame.RewVideoShow(77);
        _claimX2.gameObject.SetActive(false);
    }

    private void OnRewardedShown(int n)
    {
        if (n == 77)
        {
            _reward *= 2;
            _rewardCount.text = _reward.ToString();
        }
    }
}
