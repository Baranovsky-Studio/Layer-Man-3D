using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace BaranovskyStudio
{
    [AddComponentMenu("Arcade Idle Components/Settings Panel", 0)]
    public class SettingsPanel : UIElement
    {
        [InfoBox("Button close is necessary only for big settings panel.")]
        [BoxGroup("LINKS")] [SerializeField] 
        private Button _close;
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop sounds button here.")] 
        private Button _sounds;

        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop sounds on sprite here.")] 
        private Sprite _soundsOn;
        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop sounds off sprite here.")] 
        private Sprite _soundsOff;

        private void Start()
        {
            AudioListener.volume = Bootstrap.Instance.PlayerData.Sounds ? 1f : 0f;

            CheckForErrors();
            
            if (_close != null)
            {
                _close.onClick.AddListener(HideElement);
            }

            _sounds.onClick.AddListener(OnButtonSoundsClick);
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_sounds == null)
                {
                    Debug.LogError("Settings Panel: it seems like one of settings button is null.");
                }

                if (_soundsOn == null || _soundsOff == null)
                {
                    Debug.LogError("Settings Panel: it seems like one of button's sprite is null.");
                }
            }
        }

        protected override void OnShow()
        {
            UpdateSprites();
        }

        protected override void OnHide() {}

        private void UpdateSprites()
        {
            if (Bootstrap.Instance == null) return;
            _sounds.image.sprite = Bootstrap.Instance.PlayerData.Sounds ? _soundsOn : _soundsOff;
        }

        private void OnButtonSoundsClick()
        {
            Bootstrap.Instance.PlayerData.Sounds = !Bootstrap.Instance.PlayerData.Sounds;
            Bootstrap.Instance.SaveGame();

            AudioListener.volume = Bootstrap.Instance.PlayerData.Sounds ? 1f : 0f;
            UpdateSprites();
        }
    }
}