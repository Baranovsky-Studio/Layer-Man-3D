using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using NaughtyAttributes;
using Source.Scripts;
using Source.Scripts.Game;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBackpack : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private GameObject _prefab;
    [BoxGroup("LINKS")] [SerializeField] 
    public Transform Parent;
    [BoxGroup("LINKS")] [SerializeField] 
    private Material[] _materials;
    [BoxGroup("LINKS")] [SerializeField] 
    private TextMeshPro _text;
    [BoxGroup("LINKS")] [SerializeField] 
    private TextMeshPro _text2;

    [BoxGroup("SPAWNER")] [SerializeField] 
    private float _minScale;
    [BoxGroup("SPAWNER")] [SerializeField] 
    private float _scaleDifference;

    [BoxGroup("SPAWNER")] [SerializeField] 
    private Mesh _circle_0;
    [BoxGroup("SPAWNER")] [SerializeField] 
    private Mesh _circle_1;

    [BoxGroup("SETTINGS")] [SerializeField] 
    private bool _isPlayer;
    [BoxGroup("SETTINGS")] [SerializeField] 
    private int _standartCirclesCount;
    [BoxGroup("SETTINGS")] [SerializeField] 
    private float _backpackOffset;
    [BoxGroup("SETTINGS")] [SerializeField]
    private Settings _settings;
    
    [BoxGroup("FINISH")] [SerializeField]
    private float _increaseCircleOffset;
    [BoxGroup("FINISH")] [SerializeField]
    private float _maxCircleOffset;
    
    public int CirclesCount => SpawnedCircles.Count;
    
    private float _currentMultiplier;
    private float _defaultLocalPosY;

    public readonly List<Circle> SpawnedCircles  = new List<Circle>();
    
    private float _currentOffset;
    private bool _isRolling;

    private AudioSource _source;
    private PlayerRagdoll _ragdoll;

    private int _circleSkinId;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _ragdoll = GetComponent<PlayerRagdoll>();
        
        UpdateCircleSkin();

        _defaultLocalPosY = Parent.localPosition.y;
        AddCircles(_isPlayer ? Bootstrap.Instance.GameData.StandartCircles : _standartCirclesCount, false);
    }

    public void ChangeCirclesCount(int count)
    {
        if (count > 0)
        {
           AddCircles(count, true);
        }
        else
        {
            RemoveCircles(count);
        }
    }

    public void UpdateDefaultCirclesCount(int defaultCount)
    {
        if (CirclesCount < defaultCount)
        {
            AddCircles(defaultCount - CirclesCount, false);
        }
    }

    private void Update()
    {
        if (Bootstrap.Instance.PlayerData.CircleSkinId != _circleSkinId)
        {
            UpdateCircleSkin();
            UpdateCirclesSkin();
        }
    }

    public void UpdateCircleSkin()
    {
        _circleSkinId = Bootstrap.Instance.PlayerData.CircleSkinId;
        _minScale = _settings.CirclesSettings[_circleSkinId].MinScale;
        _circle_0 = _settings.CirclesSettings[_circleSkinId].Normal;
        _circle_1 = _settings.CirclesSettings[_circleSkinId].Nano;
    }

    private void UpdateCirclesSkin()
    {
        for (var n = SpawnedCircles.Count - 1; n >= 0; n--) 
        {
            Destroy(SpawnedCircles[n].gameObject);
            SpawnedCircles.RemoveAt(n);
        }

        AddCircles(Bootstrap.Instance.GameData.StandartCircles, false);
    }

    private void AddCircles(int count, bool isGameplay)
    {
        var material = _materials[Random.Range(0, _materials.Length)];
        
        for (var n = 0; n < count; n++) 
        {
            var circle = Instantiate(_prefab, Parent).GetComponent<Circle>();
            var scale =  CalculateScale();

            circle.InBackpack = true;
            circle.MeshFilter.mesh = CirclesCount <= 5 ? _circle_0 : _circle_1;
            circle.Renderer.material = material;
            circle.transform.localScale = new Vector3(scale, scale, circle.transform.localScale.z);
            
            SpawnedCircles.Add(circle);
            _text.text = SpawnedCircles.Count.ToString();
            _text2.text = SpawnedCircles.Count.ToString();
            Parent.localPosition = new Vector3(0f, _defaultLocalPosY + SpawnedCircles.Count * _backpackOffset, 0f);

            if (_source != null && isGameplay)
            {
                _source.Play();
            }
        }
    }

    private void RemoveCircles(int count)
    {
        var decreasedCount = Mathf.Clamp(SpawnedCircles.Count + count, 0, Int32.MaxValue);
        while (SpawnedCircles.Count > decreasedCount)
        { 
            SpawnedCircles[SpawnedCircles.Count - 1].ThrowAway();
            SpawnedCircles.RemoveAt(SpawnedCircles.Count - 1);
            _text.text = SpawnedCircles.Count.ToString();
            _text2.text = SpawnedCircles.Count.ToString();
            Parent.localPosition = new Vector3(0f, _defaultLocalPosY + SpawnedCircles.Count * _backpackOffset, 0f);
        }

        if (CirclesCount == 0)
        {
            OnLose();
        }
    }

    public Circle GetCircle()
    {
        var circle = SpawnedCircles[SpawnedCircles.Count - 1];
        SpawnedCircles.RemoveAt(SpawnedCircles.Count - 1);
        _text.text = SpawnedCircles.Count.ToString();
        _text2.text = SpawnedCircles.Count.ToString();

        if (CirclesCount == 0)
        {
            OnLose();
        }

        return circle;
    }

    private void OnLose()
    {
        Bootstrap.Instance.ChangeGameState(GameStateID.Lose);
        _ragdoll.UseRagdoll = true;
        _text.text = "";
        _text2.text = "";
    }

    public void AddCircle(Circle circle)
    {
        circle.MeshFilter.mesh = CirclesCount <= 5 ? _circle_0 : _circle_1;
        SpawnedCircles.Add(circle);
        _text.text = SpawnedCircles.Count.ToString();
        _text2.text = SpawnedCircles.Count.ToString();
        Parent.localPosition = new Vector3(0f, _defaultLocalPosY + SpawnedCircles.Count * _backpackOffset, 0f);
        
        if (_source != null)
        {
            _source.Play();
        }
    }

    public float CalculateScale()
    {
        return _minScale + SpawnedCircles.Count * _scaleDifference;
    }
    
    public void RollCirclesOut(Action onReady)
    {
        //if (_isRolling) return;
        
        _currentOffset += _increaseCircleOffset;
        if (_currentOffset > _maxCircleOffset) _currentOffset = _maxCircleOffset;

        onReady?.Invoke();
        StartCoroutine(RollingOut());
    }

    private IEnumerator RollingOut()
    {
        _isRolling = true;
        var delay = (Bootstrap.Instance.GetSystem<FinishingSystem>().GetFinishingDelay() - 1.5f) / SpawnedCircles.Count / 4;

        if (_currentOffset == _increaseCircleOffset)
        {
            var lastN = 0;
            for (var n = 0; n < SpawnedCircles.Count; n++)
            {
                if (n == 0)
                {
                    SpawnedCircles[n].transform.eulerAngles = Vector3.zero;
                    continue;
                }

                var position = SpawnedCircles[n - 1].transform.position + new Vector3(0f, 0f, _currentOffset);
                SpawnedCircles[n].transform.position = position;
                SpawnedCircles[n].transform.eulerAngles = Vector3.zero;

                for (var e = n + 1; e < SpawnedCircles.Count; e++)
                {
                    SpawnedCircles[e].transform.position = position;
                }

                Parent.localPosition = new Vector3(0f, _defaultLocalPosY + _backpackOffset * (SpawnedCircles.Count - n), 0f);

                if (n == lastN + 2)
                {
                    yield return new WaitForSeconds(delay * 2);
                    lastN = n;
                }
            }
        }
        else
        {
            foreach (var circle in SpawnedCircles)
            {
                circle.PlayAnim();
                yield return new WaitForFixedUpdate();
            }
        }

        _isRolling = false;
        _text.text = "";
        _text2.text = "";
    }

    public void RollCirclesIn()
    {
        StartCoroutine(RollingIn());
    }

    private IEnumerator RollingIn()
    {
        foreach (var circle in SpawnedCircles)
        {
            circle.transform.parent = null;
        }

        var lastN = 0;
        for (var n = 0; n < SpawnedCircles.Count - 1; n++)
        {
            SpawnedCircles[n].transform.position = SpawnedCircles[n + 1].transform.position;
            for (var e = 0; e < n; e++)
            {
                SpawnedCircles[e].transform.position = SpawnedCircles[n].transform.position;
            }
            
            if (n == lastN + 4)
            {
                yield return new WaitForSeconds(0.2f / SpawnedCircles.Count * 4);
                lastN = n;
            }
        }
    }

    public void SetDefaultHeadPosition()
    {
        Parent.localPosition = new Vector3(0f, _defaultLocalPosY + _backpackOffset * 0, 0f);
    }
}
