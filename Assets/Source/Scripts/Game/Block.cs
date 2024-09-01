using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class Block : MonoBehaviour
{
    [BoxGroup("THROWING")] [SerializeField]
    private Rigidbody[] _blocks;
    [BoxGroup("THROWING")] [SerializeField] 
    private float _power;
    
    [BoxGroup("MOVING")] [SerializeField] 
    private bool _useMoving;
    [BoxGroup("MOVING")] [SerializeField] [ShowIf("_useMoving")]
    private Vector3 _startPos;
    [BoxGroup("MOVING")] [SerializeField] [ShowIf("_useMoving")]
    private Vector3 _endPos;
    [BoxGroup("MOVING")] [SerializeField] [ShowIf("_useMoving")]
    private float _speed;

    [BoxGroup("SETTINGS")] [SerializeField]
    private int _circlesCount;

    private bool _used;

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

    private void Start()
    {
        StartCoroutine(MovingLoop());
    }

    private IEnumerator MovingLoop()
    {
        while (_useMoving)
        {
            while (Vector3.Distance(transform.position, _startPos) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _startPos, _speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while (Vector3.Distance(transform.position, _endPos) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _endPos, _speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_used) return;

            var backpack = other.gameObject.GetComponent<PlayerBackpack>();
            if (backpack.CirclesCount > _circlesCount)
            {
                foreach (var block in _blocks)
                {
                    block.isKinematic = false;
                    block.AddForce(new Vector3(Random.Range(-1f, 1f),Random.Range(0.1f, 0.4f),Random.Range(0.4f, 1f)) * _power, ForceMode.Impulse);
                }

                StartCoroutine(WaitForTurnOff());
            }
            backpack.ChangeCirclesCount(-_circlesCount);
            
            _used = true;
        }
    }

    private IEnumerator WaitForTurnOff()
    {
        yield return new WaitForSeconds(1f);
        foreach (var block in _blocks)
        {
            block.gameObject.SetActive(false);
        }
    }
}
