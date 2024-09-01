using System.Collections;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Game
{
    public class Circle : MonoBehaviour
    {
        [BoxGroup("SETTINGS")] [SerializeField]
        private float _speed;
        [BoxGroup("SETTINGS")] [SerializeField]
        private float _lifeTime;
        [BoxGroup("SETTINGS")] [SerializeField]
        private Settings _settings;

        public bool InBackpack;
        public MeshRenderer Renderer;
        public MeshFilter MeshFilter;
        [HideInInspector]
        public Rigidbody Rigidbody;

        private Animator _anim;
        private int _circleSkinId;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _anim = GetComponentInChildren<Animator>();
            MeshFilter.mesh = _settings.CirclesSettings[Bootstrap.Instance.PlayerData.CircleSkinId].Big;
        }

        private void Update()
        {
            if (InBackpack) return;
            if (Bootstrap.Instance.PlayerData.CircleSkinId != _circleSkinId)
            {
                UpdateCircleSkin();
            }
        }

        private void UpdateCircleSkin()
        {
            _circleSkinId = Bootstrap.Instance.PlayerData.CircleSkinId;
            MeshFilter.mesh = _settings.CirclesSettings[Bootstrap.Instance.PlayerData.CircleSkinId].Big;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (InBackpack) return;
            
            if (other.CompareTag("Player"))
            {
                StartCoroutine(MovingToBackpackLoop(other.GetComponent<PlayerBackpack>()));
            }
        }

        public void MoveToBackpack(PlayerBackpack playerBackpack)
        {
            StartCoroutine(MovingToBackpackLoop(playerBackpack));
        }

        private IEnumerator MovingToBackpackLoop(PlayerBackpack playerBackpack)
        {
            InBackpack = true;
            playerBackpack.AddCircle(this);

            var scale = playerBackpack.CalculateScale();
            var localScale = new Vector3(scale, scale, 1f);

            transform.SetParent(playerBackpack.Parent);

            var uppedLocalPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            var increasedLocalScale = localScale * 1.4f;
            
            while (Vector3.Distance(transform.localPosition, uppedLocalPosition) > 0.01f || Vector3.Distance(transform.localScale, increasedLocalScale) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, uppedLocalPosition, _speed * Time.deltaTime);
                transform.localScale = Vector3.MoveTowards(transform.localScale, increasedLocalScale, _speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            while (Vector3.Distance(transform.localPosition, Vector3.zero) > 0.01f || Vector3.Distance(transform.localScale, localScale) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, _speed * Time.deltaTime);
                transform.localScale = Vector3.MoveTowards(transform.localScale, localScale, _speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = localScale;
        }

        public void ThrowAway()
        {
            transform.parent = null;
            
            Rigidbody.isKinematic = false;

            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
            Rigidbody.velocity = direction * 10f;

            StartCoroutine(TurnOffTimer());
        }

        private IEnumerator TurnOffTimer()
        {
            yield return new WaitForSeconds(_lifeTime);
            
            Rigidbody.isKinematic = true;
            Renderer.enabled = false;
        }

        public void PlayAnim()
        {
            _anim.SetTrigger("Sizing");
        }
    }
}