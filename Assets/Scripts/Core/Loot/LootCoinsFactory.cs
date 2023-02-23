using Core.Managers;
using Pooling;
using UnityEngine;
using Zenject;

namespace Core.Loot
{
    public class LootCoinsFactory : MonoBehaviour
    {
        [Inject] private PlayerMovement _player;
        [Inject] private GameStats _gameStats;
        [SerializeField] private DropCoin coinPrefab;

        public void SpawnCoins(int count, Vector3 position)
        {
            for (var i = 0; i < count; i++)
            {
                var rndCircle = Random.insideUnitCircle;
                var coin = PoolManager.Spawn(
                    coinPrefab, 
                    position + new Vector3(rndCircle.x, 0, rndCircle.y),
                    Quaternion.identity);
                coin.Setup(_player.transform, this);
            }
        }

        public void CollectCoin()
        {
            _gameStats.AddCoins(1);
        }
    }
}