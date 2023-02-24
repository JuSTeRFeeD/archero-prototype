using System;
using System.Collections;
using Core.Map;
using UnityEngine;
using Zenject;

namespace Core.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerMovement _playerMovement;
        [Inject] private MapGrid _mapGrid;

        [SerializeField] private FollowCamera followCamera;
        [SerializeField] private Gates gates;
        
        public bool IsPaused { get; private set; }
        public bool IsGameOver { get; private set; }
        public int RoomNumber { get; private set; } = 1;
        public event Action OnUpdateRoom;
        public event Action OnDefeat;
        
        public void SetPause(bool value)
        {
            IsPaused = value;
            Time.timeScale = IsPaused ? 0 : 1;
        }

        private void Start()
        {
            _playerMovement.GetComponent<Entity>().OnDeath += _ => HandlePlayerDeath();
            gates.OnPlayerFinish += () => StartCoroutine(nameof(StartUpdateRoom));
        }

        private void HandlePlayerDeath()
        {
            IsGameOver = true;
            OnDefeat?.Invoke();
        }

        private IEnumerator StartUpdateRoom()
        {
            if (IsGameOver) yield break;
            
            _sceneLoader.SetActiveFade(true);
            
            yield return new WaitForSeconds(SceneLoader.FadeTime);
            
            _playerMovement.transform.position = _mapGrid.GetPlayerSpawnPosition();
            followCamera.transform.position = _playerMovement.transform.position;
            RoomNumber++;
            OnUpdateRoom?.Invoke();
            
            _sceneLoader.SetActiveFade(false);
            yield return new WaitForSeconds(SceneLoader.FadeTime);
            
        }
    }
}
