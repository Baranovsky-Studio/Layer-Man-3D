using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class UnlockableBox : MonoBehaviour
    {
        [BoxGroup("MATERIALS")] [SerializeField]
        private Material _locked;
        [BoxGroup("MATERIALS")] [SerializeField]
        private Material _unlocked;
        [SerializeField] 
        private UnlockableItem _unlockableItem;

        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _unlockableItem.OnUnlockItem.AddListener(OnUnlockItem);
        }

        public void OnUnlockItem()
        {
            _meshRenderer.material = _unlockableItem.IsUnlocked ? _unlocked : _locked;
        }
    }
}