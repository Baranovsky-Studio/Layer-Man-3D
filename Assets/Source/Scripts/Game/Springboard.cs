using BaranovskyStudio;
using NaughtyAttributes;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [BoxGroup("SETTINGS")] [SerializeField]
    private float _power;
    [BoxGroup("SETTINGS")] [SerializeField]
    private Vector3 _direction;

    private Animator _animator;
    private AudioSource _source;
    private bool _used;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_used) return;

            _animator.SetTrigger("Jump");
            _source.Play();
            other.gameObject.GetComponent<PlayerMovement>().Jump(_power * _direction);
            _used = true;
        }
    }
}
