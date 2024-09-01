using System.Collections;
using NaughtyAttributes;
using Source.Scripts.Game;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private Transform _hook;
    [BoxGroup("SETTINGS")] [SerializeField]
    private float _speed;
    
    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_used) return;

            var circle = other.GetComponent<PlayerBackpack>().GetCircle();
            
            circle.StopAllCoroutines();
            
            circle.transform.parent = transform;
            circle.Rigidbody.isKinematic = true;
            
            StartCoroutine(MovingLoop(circle));
            _used = true;
        }
    }

    private IEnumerator MovingLoop(Circle circle)
    {
        while (Vector3.Distance(_hook.position, circle.transform.position) > 0.2f || Vector3.Distance(circle.transform.localEulerAngles, new Vector3(0f, 90f, 0f)) > 1f)
        {
            circle.transform.position = Vector3.MoveTowards(circle.transform.position, _hook.transform.position - new Vector3(0f, circle.transform.localScale.y / 2, 0f), _speed * Time.deltaTime);            circle.transform.localEulerAngles = Vector3.MoveTowards(circle.transform.localEulerAngles, new Vector3(0f, 90f, 0f), _speed);
            circle.transform.localEulerAngles = Vector3.MoveTowards(circle.transform.localEulerAngles, new Vector3(0f, 90f, 0f), _speed);
            yield return new WaitForEndOfFrame();
        }
    }
}
