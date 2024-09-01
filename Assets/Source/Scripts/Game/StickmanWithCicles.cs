using System.Collections;
using BaranovskyStudio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class StickmanWithCicles : MonoBehaviour
{
    [BoxGroup("SETTINGS")] [SerializeField]
    private Vector3 _startPos;
    [BoxGroup("SETTINGS")] [SerializeField]
    private Vector3 _endPos;
    [BoxGroup("SETTINGS")] [SerializeField]
    private int _circles;
    [BoxGroup("SETTINGS")] [SerializeField]
    private bool _isRed;

    private bool _used;
    private PlayerBackpack _playerBackpack;
    private PlayerAnimations _playerAnimations;
    private NavMeshAgent _agent;
    private AudioSource _source;

    [Button]
    public void SetStartPos()
    {
        _startPos = transform.position;
    }
    
    [Button]
    public void SetEndPos()
    {
        _endPos = transform.position;
    }

    private void Awake()
    {
        _playerBackpack = GetComponent<PlayerBackpack>();
        _playerAnimations = GetComponent<PlayerAnimations>();
        _agent = GetComponent<NavMeshAgent>();
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        _playerAnimations.UpdateAnimator(1f);
        
        while (!_used)
        {
            _agent.SetDestination(_startPos);
            yield return new WaitWhile(() => Vector3.Distance(transform.position, _startPos) > 0.5f);
            _agent.SetDestination(_endPos);
            yield return new WaitWhile(() => Vector3.Distance(transform.position, _endPos) > 0.5f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_used) return;

            var backpack = other.gameObject.GetComponent<PlayerBackpack>();

            if (_isRed == false)
            {
                foreach (var circle in _playerBackpack.SpawnedCircles)
                {
                    circle.transform.parent = backpack.Parent;
                    circle.MoveToBackpack(backpack);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                backpack.ChangeCirclesCount(-_circles);
                if (_source != null)
                {
                    _source.Play();
                }
            }
            
            _used = true;
        }
    }
}
