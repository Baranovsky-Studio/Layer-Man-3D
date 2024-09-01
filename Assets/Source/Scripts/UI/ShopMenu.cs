using NaughtyAttributes;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [BoxGroup("LINKS")] [SerializeField] 
    private ShopButton[] _buttons;

    private void Start()
    {
        foreach (var button in _buttons)
        {
            button.OnSelect += OnButtonClick;
        }
    }

    private void OnButtonClick(ShopButton selectedButton)
    {
        foreach (var button in _buttons)
        {
            if (button.Equals(selectedButton)) continue;
            button.OnDeselect();
        }
    }
}
