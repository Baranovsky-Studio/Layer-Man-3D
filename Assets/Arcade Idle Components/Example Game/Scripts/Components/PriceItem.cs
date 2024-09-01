using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    public class PriceItem : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] 
        private TextMeshProUGUI _price;

        public void SetPrice(int price)
        {
            _price.text = price.ToString();
        }
    }
}