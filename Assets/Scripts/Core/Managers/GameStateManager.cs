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
            ResetPlayerAndCameraPosition();
            _playerMovement.GetComponent<Entity>().OnDeath += HandlePlayerDeath;
            gates.OnPlayerFinish += UpdateRoom;
        }

        private void OnDestroy()
        {
            OnDefeat = null;
            OnUpdateRoom = null;
        }

        private void HandlePlayerDeath(Entity e)
        {
            IsGameOver = true;
            OnDefeat?.Invoke();
        }

        private void ResetPlayerAndCameraPosition()
        {
            _playerMovement.transform.position = _mapGrid.PlayerSpawnPosition;
            followCamera.transform.position = _playerMovement.transform.position;
        }

        private void UpdateRoom()
        {
            StartCoroutine(nameof(StartUpdateRoom));
        }
        
        private IEnumerator StartUpdateRoom()
        {
            if (IsGameOver) yield break;
            
            _sceneLoader.SetActiveFade(true);
            
            yield return new WaitForSeconds(SceneLoader.FadeTime);
            
            ResetPlayerAndCameraPosition();
            RoomNumber++;
            OnUpdateRoom?.Invoke();
            
            _sceneLoader.SetActiveFade(false);
            yield return new WaitForSeconds(SceneLoader.FadeTime);
            
        }
    }
}
