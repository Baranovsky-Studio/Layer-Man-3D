using Kuhpik;
using NaughtyAttributes;
using UnityEngine;

public class PlayerAdditionals : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject[] _trails;
    [BoxGroup("LINKS")] [SerializeField] 
    private Material[] _materials;
    [BoxGroup("LINKS")] [SerializeField] 
    private SkinnedMeshRenderer _body;
    [BoxGroup("LINKS")] [SerializeField] 
    private MeshRenderer _head;

    private int _trailId;
    private int _materialId;
    
    private void Start()
    {
        UpdateState();
    }

    private void Update()
    {
        if (Bootstrap.Instance.PlayerData.TrailId != _trailId || Bootstrap.Instance.PlayerData.MaterialId != _materialId)
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        _trailId = Bootstrap.Instance.PlayerData.TrailId;
        _materialId = Bootstrap.Instance.PlayerData.MaterialId;
        
        if (Bootstrap.Instance.PlayerData.TrailId > 0)
        {
            foreach (var trail in _trails)
            {
                trail.SetActive(false);
            }
            _trails[Bootstrap.Instance.PlayerData.TrailId - 1].SetActive(true);
        }
        else
        {
            foreach (var trail in _trails)
            {
                trail.SetActive(false);
            }
        }

        _head.material = _materials[Bootstrap.Instance.PlayerData.MaterialId];
        _body.material = _materials[Bootstrap.Instance.PlayerData.MaterialId];
    }
}
