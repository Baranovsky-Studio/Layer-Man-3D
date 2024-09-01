using UnityEngine;
using UnityEngine.UI;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Button Link", 0)]
    public class ButtonLink : MonoBehaviour
    {
        [SerializeField] 
        private string _url;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Application.OpenURL(_url);
        }
    }
}