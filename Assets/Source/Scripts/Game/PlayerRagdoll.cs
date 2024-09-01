using BaranovskyStudio;
using NaughtyAttributes;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField]
    private Rigidbody[] _rigidbodies;
    [BoxGroup("LINKS")] [SerializeField]
    private Rigidbody _mainRigidbody;
    [BoxGroup("LINKS")] [SerializeField]
    private Collider[] _colliders;

    private PlayerAnimations _animations;

    public bool UseRagdoll
    {
        get => _useRagdoll;
        set
        {
            _useRagdoll = value;
            ChangeRagdollState();
        }
    }
    private bool _useRagdoll;

    private void Start()
    {
        _animations = GetComponent<PlayerAnimations>();
        ChangeRagdollState();
    }

    private void ChangeRagdollState()
    {
        if (_useRagdoll)
        {
            _animations.TurnOffAnimator();
        }

        for (var n = 0; n < _rigidbodies.Length; n++)
        {
            _rigidbodies[n].isKinematic = !_useRagdoll;
            _colliders[n].enabled = _useRagdoll;
        }
    }

    public void AddForce(Vector3 force)
    {
        _mainRigidbody.AddForce(force, ForceMode.Impulse);
    }
}
