using Kuhpik;
using NaughtyAttributes;
using Pocket_Snake;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LoseUIScreen : UIScreen
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Counter _levelCounter;
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _tryAgain;
    
    private void Start()
    {
        _tryAgain.onClick.AddListener(OnButtonReplayClick);
    }

    public override void Open()
    {
        base.Open();
        _levelCounter.SetValue(Bootstrap.Instance.PlayerData.GameId);
    }

    private void OnButtonReplayClick()
    {
        Bootstrap.Instance.GetSystem<LoadLevelSystem>().LoadLevel();
        Bootstrap.Instance.ChangeGameState(GameStateID.Menu);
        YandexGame.FullscreenShow();
    }
}
