using System;
using BaranovskyStudio;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private UIElement _list;
    
    [BoxGroup("SETTINGS")] [SerializeField] 
    private Color _selected;
    [BoxGroup("SETTINGS")] [SerializeField] 
    private Color _deselected;

    [BoxGroup("TEXT")] [SerializeField] 
    private TextMeshProUGUI _text;
    [BoxGroup("TEXT")] [SerializeField] 
    private Color _textSelected;
    [BoxGroup("TEXT")] [SerializeField] 
    private Color _textDeselected;

    public Action<ShopButton> OnSelect;
    private Button _button;
    
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _button.image.color = _selected;
        _text.color = _textSelected;
        _list.ShowElement();
        OnSelect?.Invoke(this);
    }

    public void OnDeselect()
    {
        _button.image.color = _deselected;
        _text.color = _textDeselected;
        _list.HideElement();
    }
}
