using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Pocket_Snake
{
    public class LoadLevelSystem : GameSystemWithScreen<MenuUIScreen>
    {
        [BoxGroup("TEST MODE")] [SerializeField] 
        private int _testLevelId;

        private bool _gameReady;
        
        public override void OnInit()
        {
            LoadLevel();
            base.OnInit();
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(_testLevelId != 0 ? _testLevelId : player.LevelId);
        }

        public void OnCameraReady()
        {
            screen.OnCameraReady();
            
            if (_gameReady) return;
            
            YandexGame.Instance.FirstСalls();
            _gameReady = true;
        }
    }
}
