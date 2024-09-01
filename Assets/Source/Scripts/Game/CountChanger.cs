using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class CountChanger : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private TextMeshPro _text;
    [BoxGroup("LINKS")] [SerializeField] 
    private MeshRenderer _meshRenderer;

    [BoxGroup("SETTINGS")] [SerializeField]
    private Material _red;
    [BoxGroup("SETTINGS")] [SerializeField]
    private Material _green;
    [BoxGroup("SETTINGS")] [SerializeField]
    private int _count;

    public Action OnUsed;
    public bool Used { get; set; }

    private void Start()
    {
        if (_count > 0)
        {
            _text.text = $"+${_count}";
            _meshRenderer.material = _green;
        }
        else
        {
            _text.text = $"-${-_count}";
            _meshRenderer.material = _red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Used) return;
            //other.gameObject.GetComponent<Backpack>().ChangeCirclesCount(_count);
            Used = true;
        }
    }
}
