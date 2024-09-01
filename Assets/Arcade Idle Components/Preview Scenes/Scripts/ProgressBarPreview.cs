using System.Collections;
using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class ProgressBarPreview : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private ProgressBar _progressBar;

        private void Start()
        {
            _progressBar = GetComponent<ProgressBar>();
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            var maxValue = 20f;
            var value = maxValue;

            while (true)
            {
                if (value < 0)
                {
                    value = maxValue;
                }
                else
                {
                    value -= _speed * Time.deltaTime;
                }
                
                _progressBar.SetProgress(value, maxValue);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}