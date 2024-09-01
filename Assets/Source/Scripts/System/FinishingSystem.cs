using System.Collections;
using BaranovskyStudio;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;

public class FinishingSystem : GameSystemWithScreen<FinishingUIScreen>
{
    [BoxGroup("SETTINGS")] [SerializeField]
    private float _rollingTime;
    private PlayerBackpack _backpack;
    private PlayerMovement _movement;
    private int _rollingsCount;
    
    public override void OnInit()
    {
        base.OnInit();
        screen.OnTapClick += OnTapClick;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
        var player = GameObject.FindGameObjectWithTag("Player");
        _backpack = player.GetComponent<PlayerBackpack>();
        _movement = player.GetComponent<PlayerMovement>();

        _rollingsCount = 1;
        
        StartCoroutine(Timer());
    }

    private void OnTapClick()
    {
        _backpack.RollCirclesOut(() =>
        {
            if (_rollingsCount < 4)
            {
                _rollingsCount++;
            }
        });
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_rollingTime);
        screen.OnRolling();
        _backpack.RollCirclesIn();
        yield return new WaitForSeconds(0.2f);
        _movement.FlyTo(_rollingsCount);
    }

    public float GetFinishingDelay()
    {
        return _rollingTime;
    }
}
