using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class BackpackItemsAdder : MonoBehaviour
    {
        [SerializeField] private bool _isAdder;
        [SerializeField] private int _itemsCount;
        
        private void Start()
        {
            GetComponent<Trigger>().OnEnterTrigger.AddListener(OnPlayerTriggerEnter);
        }

        private void OnPlayerTriggerEnter(GameObject player)
        {
            player.GetComponent<Backpack>().ItemsCount += _isAdder ? _itemsCount : -_itemsCount;
        }
    }
}