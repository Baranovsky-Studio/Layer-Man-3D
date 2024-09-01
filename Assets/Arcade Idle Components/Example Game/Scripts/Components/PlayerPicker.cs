using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    public class PlayerPicker : MonoBehaviour
    {
        private SpecialBackpack _specialBackpack;

        private void Start()
        {
            _specialBackpack = GetComponentInChildren<SpecialBackpack>();
            GetComponent<Trigger>().OnEnterTrigger.AddListener(TryPickUp);
        }

        private void TryPickUp(GameObject go)
        {
            if (_specialBackpack.IsBackpackFull()) return;
                
            var fruit = go.GetComponent<Fruit>();
            if (_specialBackpack.ShowedItemsType == fruit.resourceType || _specialBackpack.ItemsCount == 0)
            {
                _specialBackpack.AddItems(1, fruit.resourceType);
                fruit.OnPickup?.Invoke(fruit);
            }
        }
    }
}