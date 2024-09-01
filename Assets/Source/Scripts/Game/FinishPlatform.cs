using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField]
    private TextMeshPro _text;
    [BoxGroup("LINKS")] [SerializeField]
    private Material[] _materials;
    
    [BoxGroup("SETTINGS")]
    public float Score;

    private MeshRenderer _meshRenderer;
    private bool _used;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetText()
    {
        _text.text = $"x{Score}";
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerSpine") && !_used)
        {
            StartCoroutine(ColoringLoop());
            _used = true;
        }
    }

    private IEnumerator ColoringLoop()
    {
        while (true)
        {
            _meshRenderer.material = _materials[Random.Range(0, _materials.Length)];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
