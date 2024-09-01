using Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Pointer Arrow", 0)]
    public class PointerArrow : MonoBehaviour
    {
        private enum ReachAction
        {
            Disable,
            Destroy,
        }

        [BoxGroup("TARGET")] [SerializeField] 
        private bool _setTargetFromInspector;
        [ShowIf("_setTargetFromInspector")] [BoxGroup("TARGET")] [Required("Drag and drop target transform here.")] 
        public Transform Target;

        [BoxGroup("PLAYER")] [SerializeField] 
        private bool _setPlayerManually;
        [ShowIf("_setPlayerManually")] [BoxGroup("PLAYER")] [SerializeField] [Required("Drag and drop player transform here.")] 
        private Transform _player;
        
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _distanceToPlayer;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _reachDistance;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private Vector3 _positionOffset;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private Vector3 _eulerAnglesOffset;

        [BoxGroup("ACTIONS")] [SerializeField] 
        private ReachAction _reachAction;
        [BoxGroup("EVENTS")] [SerializeField] 
        public UnityEvent OnReach;

        private void Start()
        {
            CheckForErrors();
            if(_setPlayerManually == false)
            {
                _player = GameObject.FindWithTag(Constants.PLAYER_TAG).transform;
            }
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (Target == null)
                {
                    Debug.LogWarning("Pointer Arrow target is null.");
                }

                if (_setPlayerManually && _player == null)
                {
                    Debug.LogError("Pointer Arrow player is null.");
                }
            }
        }

        private void Update()
        {
            if (Target != null)
            {
                FollowTarget();

                if (Vector3.Distance(Target.position, _player.position) < _reachDistance)
                {
                    ReachTarget();
                }
            }
        }

        private void FollowTarget()
        {
            var direction = (Target.position - _player.position).normalized;
            transform.position = _player.position + _distanceToPlayer * direction + _positionOffset;

            var eulerAngles = Math.LookAt(Target.position, transform).eulerAngles;
            transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f) + _eulerAnglesOffset;
        }

        private void ReachTarget()
        {
            OnReach?.Invoke();

            if (_reachAction == ReachAction.Disable)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
