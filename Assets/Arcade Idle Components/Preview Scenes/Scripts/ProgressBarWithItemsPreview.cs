using System.Collections;
using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class ProgressBarWithItemsPreview : MonoBehaviour
    {
        private ProgressBarWithItems _progressBar;

        private void Start()
        {
            _progressBar = GetComponent<ProgressBarWithItems>();
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            var maxValue = 5;
            var value = 0;
            
            while (true)
            {
                if (value == maxValue)
                {
                    value = 0;
                }
                else
                {
                    value++;
                    _progressBar.SetProgress(value);
                }

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}