using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private GameStats _gameStats;
        [Inject] private GameStateManager _gameStateManager;

        [SerializeField] private TextMeshProUGUI coins;
        [Space]
        [SerializeField] private TextMeshProUGUI announcer;
        [SerializeField] private RectTransform announcerPanel;
        [SerializeField] private RectTransform pausePanel;
        [SerializeField] private RectTransform defeatPanel;
        
        private Sequence _sequence;

        private void Start()
        {
            defeatPanel.gameObject.SetActive(false);
            _gameStateManager.OnDefeat += () => defeatPanel.gameObject.SetActive(true);
            
            _gameStats.OnCoinsUpdate += UpdateCoins;

            TimerBeforeStart();
        }

        private void TimerBeforeStart()
        {
            _gameStateManager.SetPause(true);
            announcerPanel.gameObject.SetActive(true);
            _sequence = DOTween.Sequence()
                .SetUpdate(true)
                .AppendInterval(.5f)
                .AppendCallback(() => announcer.text = "3")
                .AppendInterval(1)
                .AppendCallback(() => announcer.text = "2")
                .AppendInterval(1)
                .AppendCallback(() => announcer.text = "1")
                .AppendInterval(1)
                .AppendCallback(() =>
                {
                    announcerPanel.gameObject.SetActive(false);
                    _gameStateManager.SetPause(false);
                }).Play();
        }
        
        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void UpdateCoins() => coins.text = _gameStats.CollectedCoins.ToString();

        public void SetVisiblePauseMenu(bool isVisible)
        {
            _gameStateManager.SetPause(isVisible);
            pausePanel.gameObject.SetActive(isVisible);
        }

        public void ToMainMenu()
        {
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}