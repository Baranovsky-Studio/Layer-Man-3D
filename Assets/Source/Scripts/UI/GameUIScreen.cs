using Kuhpik;
using NaughtyAttributes;
using Pocket_Snake;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScreen : UIScreen
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _replay;
    [BoxGroup("LINKS")] [SerializeField] 
    private Counter _levelCounter;
    
    private void Start()
    {
        _replay.onClick.AddListener(OnButtonReplayClick);
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
    }
}
