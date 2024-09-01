using System;
using System.Collections;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerAnimations))]
    [AddComponentMenu("Arcade Idle Components/Player Movement", 0)]
    public class PlayerMovement : MonoBehaviour
    {
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _movementSpeed = 10f;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _sidesSpeed = 8f;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _sidesEnd = 3f;
        
        [BoxGroup("FINISHING")] [SerializeField] 
        private float _forcePower = 0.1f;
        [BoxGroup("FINISHING")] [SerializeField] 
        private float _moveDistancePerCircle;
        [BoxGroup("FINISHING")] [SerializeField] 
        private float _finishingSpeed;
        [BoxGroup("FINISHING")] [SerializeField] 
        private ParticleSystem _flyingParticle;
        [BoxGroup("FINISHING")] [SerializeField] 
        private ParticleSystem  _coffettiParticle;

        private Rigidbody _rigidbody;
        private PlayerAnimations _animations;
        private PlayerRagdoll _ragdoll;
        private PlayerBackpack _backpack;

        private Boss _boss;
        private bool _atFinish;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animations = GetComponent<PlayerAnimations>();
            _ragdoll = GetComponent<PlayerRagdoll>();
            _backpack = GetComponent<PlayerBackpack>();

            _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        }

        private void FixedUpdate()
        {
            if (Bootstrap.Instance.GetCurrentGamestateID() == GameStateID.Game)
            {
                _rigidbody.velocity = new Vector3(0f ,_rigidbody.velocity.y, _rigidbody.velocity.z);
                
                if (Input.GetMouseButton(0))
                {
                    var clampedPosX = Mathf.Clamp(_rigidbody.position.x + Input.GetAxis("Mouse X") * _sidesSpeed * Time.deltaTime, -_sidesEnd, _sidesEnd);
                    _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, new Vector3(clampedPosX, _rigidbody.position.y, _rigidbody.position.z), 1f)); //Change the coordinates of the player, to those that we just calculated
                }
            
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _movementSpeed);
                _animations.UpdateAnimator(1f);
            }
            else
            {
                _animations.UpdateAnimator(0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish") && !_atFinish)
            {
                _atFinish = true;
                Bootstrap.Instance.ChangeGameState(GameStateID.Finishing);
                Camera.main.GetComponent<PlayerCamera>().SetFinishingPosition();

                _rigidbody.velocity = Vector3.zero;
                

                var point = new Vector3(0f, transform.position.y, transform.position.z);
                StartCoroutine(MoveTo(point, null, _movementSpeed));
                _animations.SetFinishState();
            }
        }

        public void Jump(Vector3 force)
        {
            _animations.JumpAnim();
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private IEnumerator MoveTo(Vector3 point, Action onComplete, float speed)
        {
            while (Vector3.Distance(transform.position, point) > 0.4f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            
            onComplete?.Invoke();
        }

        public void FlyTo(int rollingsCount)
        {
            var moveDistancePerCircle = _moveDistancePerCircle * rollingsCount;
            var direction = transform.forward * moveDistancePerCircle * _backpack.CirclesCount;
            var point = transform.position + direction;
            var finishPoint = _boss.FinishPlatform.transform.position.z - _boss.FinishPlatform.localScale.z / 2;

            if (point.z < finishPoint)
            {
                StartCoroutine(FlyToPlatform(point));
            }
            else
            {
                StartCoroutine(FlyToBoss());
            }
        }

        private IEnumerator FlyToPlatform(Vector3 point)
        {
            _backpack.SetDefaultHeadPosition();
            _animations.SetFinishingState();
            
            var distance = Vector3.Distance(transform.position, point);
            var speed = distance / 100;

            _flyingParticle.Play();
            while (Vector3.Distance(transform.position, point) > 0.1f)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, point, speed));
                yield return new WaitForFixedUpdate();
            }
            _flyingParticle.Stop();

            _rigidbody.isKinematic = true;
            _ragdoll.UseRagdoll = true;
            
            yield return new WaitForSeconds(0.2f);
            
            _ragdoll.AddForce(new Vector3(0f, 1f, 1f) * _forcePower);
            
            yield return new WaitForSeconds(0.8f);
            
            _ragdoll.AddForce(new Vector3(0f, 1f, 1f) * _forcePower);
            
            yield return new WaitForSeconds(0.5f);

            OnWin();
        }

        private IEnumerator FlyToBoss()
        {
            _backpack.SetDefaultHeadPosition();
            _animations.SetFinishingState();
            
            var distance = Vector3.Distance(transform.position, _boss.Target.position);
            var speed = distance / 100;

            _flyingParticle.Play();
            while (Vector3.Distance(transform.position, _boss.Target.position) > 0.5f)
            {
                if (Vector3.Distance(transform.position, _boss.Target.position) < 20f)
                {
                    _animations.SetAttackState();
                }
                    
                if (Vector3.Distance(transform.position, _boss.Target.position) < 4f)
                {
                    _boss.OnDie();
                }

                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _boss.Target.position, speed));
                yield return new WaitForFixedUpdate();
            }
            _flyingParticle.Stop();

            while (Vector3.Distance(transform.position, _boss.FinishPlatform.position) > 1f)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _boss.FinishPlatform.position, _finishingSpeed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
                
            _animations.DanceAnim();
            _coffettiParticle.Play();
            
            yield return new WaitForSeconds(1.5f);
            
            OnWin();
        }

        private void OnWin()
        {
            Bootstrap.Instance.GameData.Reward = _backpack.CirclesCount;
            Bootstrap.Instance.ChangeGameState(GameStateID.Win);
        }
    }
}
