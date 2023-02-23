using TMPro;
using UnityEngine;
using Zenject;

namespace Core.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Inject] private GameStats _gameStats;

        [SerializeField] private TextMeshProUGUI coins;

        private void Start()
        {
            _gameStats.OnCoinsUpdate += UpdateCoins;
        }

        private void UpdateCoins() => coins.text = _gameStats.CollectedCoins.ToString();
    }
}