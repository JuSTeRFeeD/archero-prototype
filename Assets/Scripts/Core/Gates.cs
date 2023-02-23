using Core.Enemies;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core
{
    [DisallowMultipleComponent]
    public class Gates : MonoBehaviour
    {
        [Inject] private EnemiesFactory _enemiesFactory;
        [SerializeField] private Transform model;
        [SerializeField] private float openedGatesYPos = -2f;

        private void Start()
        {
            _enemiesFactory.OnEnemiesEliminated += Open;
        }

        private void Open()
        {
            model.DOMoveY(openedGatesYPos, 1f).SetEase(Ease.InBack);
        }
    }
}
