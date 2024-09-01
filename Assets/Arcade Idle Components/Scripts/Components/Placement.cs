using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Placement", 0)]
    public class Placement : UIElement
    {
        [InfoBox("Delay is only used for OnEnterPlacement.")]
        [BoxGroup("DELAY")] [SerializeField] 
        private bool _useDelay;
        [BoxGroup("DELAY")] [ShowIf("_useDelay")] [SerializeField] 
        private float _delayTime;

        [BoxGroup("PROGRESS BAR")] [SerializeField] 
        private bool _useProgressBar;
        [BoxGroup("PROGRESS BAR")] [ShowIf("_useProgressBar")] [SerializeField] [Required("Drag and drop progress bar here.")]
        private ProgressBar _progressBar;
        [BoxGroup("PROGRESS BAR")] [ShowIf("_useProgressBar")] [SerializeField] 
        private bool _resetOnExit;
        
        [InfoBox("Placement subscribes to Trigger events automatically, you do not need to do it manually.")]
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnEnterPlacement;
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnExitPlacement;

        private void Awake()
        {
            ShowElement();
        }

        private void Start()
        {
            CheckForErrors();
            SubscribeEvents();
        }
        
        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_useDelay && _delayTime == 0)
                {
                    Debug.LogWarning("Placement: time delay is 0.");
                }

                if (_useProgressBar && _progressBar == null)
                {
                    Debug.LogError("Placement: progress bar is null.");
                }
            }
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
        }

        /// <summary>
        /// The method is called when the player enters the placement's collider.
        /// </summary>
        /// <param name="player">Link to object entered in collider.</param>
        public void OnPlayerEnter(GameObject player)
        {
            if (IsShowed == false) return;
            
            if (_useDelay)
            {
                StartCoroutine(WaitDelay(() =>
                {
                    OnEnterPlacement?.Invoke(player);
                }));
            }
            else
            {
                OnEnterPlacement?.Invoke(player);
            }
        }
        
        /// <summary>
        /// The method is called when the player exits the placement's collider.
        /// </summary>
        /// <param name="player">Link to object left the collider.</param>
        public void OnPlayerExit(GameObject player)
        {
            StopAllCoroutines(); //Stops WaitDelay coroutine 
            
            if (_useProgressBar && _resetOnExit)
            {
                _progressBar.ResetValues();
            }
            
            OnExitPlacement?.Invoke(player);
        }

        private IEnumerator WaitDelay(Action onComplete)
        {
            var time = 0f;
            
            while (time < _delayTime)
            {
                time += Time.deltaTime;
                
                if (_useProgressBar)
                {
                    _progressBar.SetProgress(time, _delayTime);
                }
                
                yield return new WaitForEndOfFrame();
            }

            onComplete?.Invoke();
        }
        
        protected override void OnShow() { }

        protected override void OnHide() { }
    }
}