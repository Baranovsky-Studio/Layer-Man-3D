using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _text;

    public void SetValue(int value)
    {
        _text.text = value.ToString();
    }
}
