using Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class LevelInfoUI : MonoBehaviour
    {
        [Inject] private GameStats _gameStats;
        
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Image bar;

        private void Start()
        {
            _gameStats.OnExpUpdate += UpdateInfo;
            _gameStats.OnLevelUp += UpdateInfo;
            UpdateInfo();
        }

        private void OnDestroy()
        {
            _gameStats.OnExpUpdate -= UpdateInfo;
            _gameStats.OnLevelUp -= UpdateInfo;
        }

        private void UpdateInfo()
        {
            level.text = $"Lv.{_gameStats.CurrentLevel}";
            bar.fillAmount = _gameStats.ProgressPercent;
        }
    }
}