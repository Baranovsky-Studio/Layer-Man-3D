using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    public class MaterialChanger : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] 
        private bool _setMeshRendererManually;
        [BoxGroup("LINKS")] [ShowIf("_setMeshRendererManually")] [SerializeField]
        private MeshRenderer _meshRenderer;
        
        [BoxGroup("SETTINGS")] [SerializeField]
        private Material _locked;
        [BoxGroup("SETTINGS")] [SerializeField]
        private Material _unlocked;

        private void Awake()
        {
            if (_setMeshRendererManually == false)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void SetMaterial(bool unlocked)
        {
            _meshRenderer.material = unlocked ? _unlocked : _locked;
        }
    }
}