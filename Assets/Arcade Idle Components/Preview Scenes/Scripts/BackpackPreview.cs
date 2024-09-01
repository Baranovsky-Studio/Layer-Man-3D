using System.Collections;
using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class BackpackPreview : MonoBehaviour
    {
        private Backpack _backpack;
        
        private void Start()
        {
            _backpack = GetComponent<Backpack>();
            StartCoroutine(Loop());
        }
        
        private IEnumerator Loop()
        {
            while (true)
            {
                if (_backpack.ItemsCount == _backpack.MaxItemsCount)
                {
                    _backpack.ItemsCount = 0;
                }
                else
                {
                    _backpack.ItemsCount++;
                }
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
