using System;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FinishingUIScreen : UIScreen
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Counter _levelCounter;
    [BoxGroup("LINKS")] [SerializeField] 
    private Button _taptap;

    public Action OnTapClick;
    
    private void Start()
    {
        _taptap.onClick.AddListener(OnButtonTapTapClick);
    }
    
    public override void Open()
    {
        base.Open();
        _levelCounter.SetValue(Bootstrap.Instance.PlayerData.GameId);
        _taptap.gameObject.SetActive(true);
    }
    
    private void OnButtonTapTapClick()
    {
        OnTapClick?.Invoke();
    }

    public void OnRolling()
    {
        _taptap.gameObject.SetActive(false);
    }
}
