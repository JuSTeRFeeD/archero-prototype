using System;
using Core.Enemies;
using Core.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core
{
    [DisallowMultipleComponent]
    public class Gates : MonoBehaviour
    {
        [Inject] private EnemiesFactory _enemiesFactory;
        [Inject] private GameStateManager _gameStateManager;
        
        [SerializeField] private Transform model;
        [SerializeField] private float openedGatesYPos = -2f;
        [Space]
        [SerializeField] private TextMeshProUGUI roomNumber;

        private bool _isOpened = true;
        private float _initGatesPosY;
        private Sequence _sequence;
        
        public event Action OnPlayerFinish;


        private void Start()
        {
            _initGatesPosY = model.position.y;
            _enemiesFactory.OnEnemiesEliminated += () => SetOpened(true);
            _gameStateManager.OnUpdateRoom += () => SetOpened(false);
            _gameStateManager.OnUpdateRoom += UpdateRoomNumber;
            UpdateRoomNumber();
        }

        private void UpdateRoomNumber()
        {
            roomNumber.text = _gameStateManager.RoomNumber.ToString();
        }

        private void SetOpened(bool value)
        {
            _isOpened = value;
            _sequence = DOTween.Sequence()
                .Append(model.DOMoveY(_isOpened ? openedGatesYPos : _initGatesPosY, 1f))
                .SetEase(Ease.InBack);
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerFinish?.Invoke();
            }
        }
    }
}
