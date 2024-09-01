using Kuhpik;
using NaughtyAttributes;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject[] _prefabs;
    [BoxGroup("SETTINGS")] [SerializeField] 
    private Transform _finishPlatform;
    
    private void Awake()
    {
        var boss = Instantiate(_prefabs[Bootstrap.Instance.PlayerData.BossId], transform).GetComponent<Boss>();
        boss.FinishPlatform = _finishPlatform;
        boss.OnDied += () =>
        {
            Bootstrap.Instance.PlayerData.BossId += 1;
            if (Bootstrap.Instance.PlayerData.BossId >= _prefabs.Length)
            {
                Bootstrap.Instance.PlayerData.BossId = 0;
            }
            Bootstrap.Instance.SaveGame();
        };
    }
}
