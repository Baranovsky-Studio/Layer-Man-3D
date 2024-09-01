using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    public class Tree : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] 
        private MaterialChanger[] _materialChangers;
        [BoxGroup("LINKS")] [SerializeField] 
        private Transform[] _fruitSpawnPoints;
        [BoxGroup("LINKS")] [SerializeField] 
        private ProgressBar _progressBar;
        
        [BoxGroup("SETTINGS")] [SerializeField] 
        private GameObject _fruitPrefab;

        public float GrowTime;
        
        private bool _isUnlocked;
        private readonly List<Fruit> _spawnedFruit = new List<Fruit>();
        
        private void Start()
        {
            var unlockableItem = GetComponent<UnlockableItem>();
            
            if (unlockableItem.IsUnlocked)
            {
                OnUnlock();
            }
            else
            {
                unlockableItem.OnUnlockItem.AddListener(OnUnlock);
                ChangeMaterials(_isUnlocked);
            }
        }
        
        private void OnUnlock()
        {
            _isUnlocked = true;
            StartCoroutine(SpawnLoop());
            ChangeMaterials(true);
        }

        private IEnumerator SpawnLoop()
        {
            var delay = GrowTime / _fruitSpawnPoints.Length;

            while (true)
            {
                StartCoroutine(SmoothTimer());

                while (_spawnedFruit.Count != _fruitSpawnPoints.Length)
                {
                    SpawnFruit();

                    yield return new WaitForSeconds(delay);
                }
                
                foreach (var fruit in _spawnedFruit)
                {
                    fruit.OnPickup += OnPickup;
                    fruit.FallDown();
                }
                
                yield return new WaitWhile(() => _spawnedFruit.Count != 0);
            }
        }
        
        private void ChangeMaterials(bool isUnlocked)
        {
            foreach (var materialChanger in _materialChangers)
            {
                materialChanger.SetMaterial(isUnlocked);
            }
        }

        private IEnumerator SmoothTimer()
        {
            _progressBar.ShowElement();
            var time = GrowTime;
            var maxTime = GrowTime;
            
            while (time > 0)
            {
                time -= Time.deltaTime;
                _progressBar.SetProgress(time, maxTime);
                yield return new WaitForEndOfFrame();
            }
            
            _progressBar.HideElement();
        }
        
        private void SpawnFruit()
        {
            var fruit = Instantiate(_fruitPrefab, _fruitSpawnPoints[_spawnedFruit.Count]).GetComponent<Fruit>();
            _spawnedFruit.Add(fruit);
        }

        private void OnPickup(Fruit fruit)
        {
            _spawnedFruit.Remove(fruit);
            Destroy(fruit.gameObject);
        }
    }
}