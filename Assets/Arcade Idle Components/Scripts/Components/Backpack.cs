using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace BaranovskyStudio
{
    //Backpack is a component which created to create stacks of any items, with any number of columns items
    [AddComponentMenu("Arcade Idle Components/Backpack", 0)]
    public class Backpack : MonoBehaviour
    {
        [BoxGroup("SPAWNER")] [SerializeField] [Required("Drag and drop backpack item prefab here.")]
        private GameObject _prefab;
        [BoxGroup("SPAWNER")] [SerializeField] 
        private Vector3 _itemsOffset;

        [InfoBox("You can use one or more columns of elements. To do this, add links to the starting transform of each row.")]
        [BoxGroup("COLUMN")] [SerializeField] 
        private Transform[] _columns;
        [BoxGroup("COLUMN")] [OnValueChanged("OnMaxItemsCountPerColumnChanged")] [SerializeField] 
        private int _maxItemsPerColumn;

        [BoxGroup("SETTINGS")] [OnValueChanged("OnDefaultItemsCountChanged")] [SerializeField] 
        private int _defaultItemsCount;

        public int MaxItemsCount { get; private set; }
        public int ItemsCount 
        { 
            get => _itemsCount;
            set
            {
                _itemsCount = Mathf.Clamp(value, 0, MaxItemsCount);
                ShowItems();
            }
        }

        private int _itemsCount;
        private readonly List<GameObject> _spawnedItems = new List<GameObject>();
        
        private void OnDefaultItemsCountChanged()
        {
            MaxItemsCount = _columns.Length * _maxItemsPerColumn; //Calculates MaxItemsCount depends on columns count
            _defaultItemsCount = Mathf.Clamp(_defaultItemsCount, 0, MaxItemsCount); //The clamps DefaultItemCount depends on max items count
        }

        private void OnMaxItemsCountPerColumnChanged()
        {
            _maxItemsPerColumn = Mathf.Clamp(_maxItemsPerColumn, 0, int.MaxValue); //MaxItemsPerColumn can't be less then zero 
        }

        private void Start()
        {
            CheckForErrors();
            MaxItemsCount = _columns.Length * _maxItemsPerColumn; //Calculates MaxItemsCount depends on columns count
            SpawnItems(); //Creates pool of items
            ItemsCount = _defaultItemsCount; //Sets DefaultItemsCount and shows this items
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_prefab == null)
                {
                    Debug.LogError("Backpack: prefab is null.");
                }
                if (_columns.Length == 0)
                {
                    Debug.LogError("Backpack: columns length is zero.");
                }
            }
        }

        private void SpawnItems()
        {
            foreach (var column in _columns) //Spawns for every one column
            {
                var remainingItemsCount = MaxItemsCount - _spawnedItems.Count;
                var itemsForSpawnCount = remainingItemsCount >= _maxItemsPerColumn ? _maxItemsPerColumn : remainingItemsCount;
                
                SpawnItems(column, itemsForSpawnCount);
            }
        }

        private void SpawnItems(Transform column, int count)
        {
            var spawnedItems = new List<GameObject>(); //Creates a new list of items for each column to to avoid spawning item over the previous column element

            for (var n = 0; n < count; n++)
            {
                var item = Instantiate(_prefab, column);
                if (n != 0) //If this isn't empty column, sets position depend of last item position
                {
                    var lastItemPosition = spawnedItems[n - 1].transform.position;
                    item.transform.position = new Vector3(lastItemPosition.x, lastItemPosition.y + _prefab.transform.localScale.y, lastItemPosition.z) + _itemsOffset;
                }
                spawnedItems.Add(item);
            }

            _spawnedItems.AddRange(spawnedItems);
        }
        
        private void ShowItems()
        {
            for (var n = 0; n < ItemsCount; n++)
            {
                _spawnedItems[n].SetActive(true);
            }
            for (var n = ItemsCount; n < _spawnedItems.Count; n++)
            {
                _spawnedItems[n].SetActive(false);
            }
        }
    }
}