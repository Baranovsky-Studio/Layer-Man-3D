using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _prefab;
    [BoxGroup("SETTINGS")] [SerializeField] 
    private float _offset;
    [BoxGroup("SETTINGS")][SerializeField] 
    private List<FinishPlatform> _platforms;

    [Button]
    private void SetScoreToPlatforms()
    {
        var n = 10;
        foreach (var platform in _platforms)
        {
            platform.Score = n / 10f;
            platform.SetText();
            n += 1;
        }
    }

    #if UNITY_EDITOR
    [Button]
    private void SpawnPlatform()
    {
        var platform = PrefabUtility.InstantiatePrefab(_prefab, transform).GetComponent<FinishPlatform>();
        platform.transform.localPosition = _platforms[_platforms.Count - 1].transform.localPosition + new Vector3(_offset, 0f, 0f);
        
        _platforms.Add(platform);
        SetScoreToPlatforms();
    }
    #endif
}
