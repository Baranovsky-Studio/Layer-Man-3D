using System.Collections;
using Kuhpik;
using NaughtyAttributes;
using Pocket_Snake;
using UnityEngine;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Player Camera", 0)]
    public class PlayerCamera : MonoBehaviour
    {
        [BoxGroup("TARGET")] [Required("Drag and drop camera target here.")] 
        public Transform Target;
        [BoxGroup("TARGET")] 
        public Vector3 Offset;
        
        [BoxGroup("PIVOT")] [SerializeField] 
        private Transform _pivot;
        
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _startDelay;
        [BoxGroup("SETTINGS")] [SerializeField] 
        private float _movingToPlayerSpeed;

        [BoxGroup("MOVEMENT")] [SerializeField] 
        private float _movementSpeed;
        [InfoBox("You can limit on which axis to follow the player.")]
        [BoxGroup("MOVEMENT")] [SerializeField] 
        private bool _followPositionX;
        [BoxGroup("MOVEMENT")] [SerializeField] 
        private bool _followPositionY;
        [BoxGroup("MOVEMENT")] [SerializeField] 
        private bool _followPositionZ;
        
        [BoxGroup("ROTATION")] [SerializeField] 
        private float _rotationSpeed;
        [InfoBox("You can limit on which axis to follow the player.")]
        [BoxGroup("ROTATION")] [SerializeField] 
        private bool _followRotationX;
        [BoxGroup("ROTATION")] [SerializeField] 
        private bool _followRotationY;
        [BoxGroup("ROTATION")] [SerializeField] 
        private bool _followRotationZ;
        
        [BoxGroup("FINISHING")] [SerializeField] 
        public Vector3 FinishOffset;
        [BoxGroup("FINISHING")] [SerializeField] 
        public Vector3 FinishEulerAngles;
        
        private Transform _mainTransform;
        private bool _isActive;

        [Button]
        private void TryFindPlayer()
        {
            Target = GameObject.FindWithTag(Constants.PLAYER_TAG).transform;
        }
        
        [Button]
        private void TryFindPivot()
        {
            _pivot = transform.root;
        }

        private void Start()
        {
            CheckForErrors();

            if (_pivot == null)
            {
                _mainTransform = transform;
                transform.localPosition = Offset;
            }
            else
            {
                _mainTransform = _pivot;
                transform.localPosition = Offset;
            }

            StartCoroutine(Delay(_startDelay));
        }

        private IEnumerator Delay(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            while (Vector3.Distance(_mainTransform.position, Target.position) > 0.1f)
            {
                var positionX = _followPositionX ? Target.position.x : _mainTransform.position.x;
                var positionY = _followPositionY ? Target.position.y : _mainTransform.position.y;
                var positionZ = _followPositionZ ? Target.position.z : _mainTransform.position.z;
                var targetPosition = new Vector3(positionX, positionY, positionZ);
            
                _mainTransform.position = Vector3.MoveTowards(_mainTransform.position, targetPosition, _movingToPlayerSpeed * Time.timeScale);
                yield return new WaitForEndOfFrame();
            }

            _isActive = true;
            
            Bootstrap.Instance.GetSystem<LoadLevelSystem>().OnCameraReady();
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (Target == null)
                {
                    Debug.LogError("Player Camera: target is null.");
                }
            }
        }

        private void LateUpdate()
        {
            if(_isActive == false) return;
            
            if (Target != null)
            {
                if (_pivot != null) 
                {
                    UpdatePivotPosition();
                }
                else
                {
                    UpdateCameraPosition();
                }

                UpdateRotation();
            }
        }

        private void UpdatePivotPosition()
        {
            //Updates position depending on axes to be followed
            var positionX = _followPositionX ? Target.position.x : _mainTransform.position.x;
            var positionY = _followPositionY ? Target.position.y : _mainTransform.position.y;
            var positionZ = _followPositionZ ? Target.position.z : _mainTransform.position.z;
            var targetPosition = new Vector3(positionX, positionY, positionZ);
            
            _mainTransform.position = Vector3.MoveTowards(_mainTransform.position, targetPosition, _movementSpeed * Time.timeScale);
        }

        private void UpdateCameraPosition()
        {
            //Updates position depending on axes to be followed
            var positionX = _followPositionX ? Target.position.x + Offset.x : _mainTransform.position.x;
            var positionY = _followPositionY ? Target.position.y + Offset.y : _mainTransform.position.y;
            var positionZ = _followPositionZ ? Target.position.z + Offset.z : _mainTransform.position.z;
            var targetPosition = new Vector3(positionX, positionY, positionZ);
            
            _mainTransform.position = Vector3.MoveTowards(_mainTransform.position, targetPosition, _movementSpeed * Time.timeScale);
        }

        private void UpdateRotation()
        {
            //Updates angles depending on axes to be followed
            var eulerAngleX = _followRotationX ? Target.eulerAngles.x : _mainTransform.eulerAngles.x;
            var eulerAngleY = _followRotationY ? Target.eulerAngles.y : _mainTransform.eulerAngles.y;
            var eulerAngleZ = _followRotationZ ? Target.eulerAngles.z : _mainTransform.eulerAngles.z;
            var targetRotation = Quaternion.Euler(eulerAngleX, eulerAngleY, eulerAngleZ);
            
            _mainTransform.rotation = Quaternion.RotateTowards(_mainTransform.rotation, targetRotation, _rotationSpeed);
        }

        public void SetFinishingPosition()
        {
            StartCoroutine(SetPositionOffset());
            StartCoroutine(SetRotationOffset());
        }

        private IEnumerator SetPositionOffset()
        {
            while (Vector3.Distance(transform.localPosition, FinishOffset) > 0.1f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, FinishOffset, _movementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator SetRotationOffset()
        {
            while (Quaternion.Dot(transform.localRotation, Quaternion.Euler(FinishEulerAngles)) > 0.5f)
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler( FinishEulerAngles), _movementSpeed * 5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}