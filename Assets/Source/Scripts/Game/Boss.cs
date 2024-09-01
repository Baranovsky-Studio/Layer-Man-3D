
using System;
using NaughtyAttributes;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [BoxGroup("LINKS")]
    public Transform Target;
    public Transform FinishPlatform;

    public Action OnDied;
    
    private Animator _animator;
    private bool _died;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnDie()
    {
        if (_died) return;
        _animator.SetBool("Died", true);
        OnDied?.Invoke();
        _died = true;
    }
}
