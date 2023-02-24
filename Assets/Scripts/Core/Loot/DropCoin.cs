using Pooling;
using UnityEngine;

namespace Core.Loot
{
    [DisallowMultipleComponent]
    public class DropCoin : Poolable
    {
        private const float CollectSpeed = 12f;
        private const float DistanceToCollect = 1.25f;
        
        private Transform _player;
        private LootCoinsFactory _coinsFactory;

        public void Setup(Transform player, LootCoinsFactory coinsFactory)
        {
            _coinsFactory = coinsFactory;
            _player = player;
        }

        private void Update()
        {
            if ((transform.position - _player.position).sqrMagnitude > DistanceToCollect)
            {
                transform.position += (_player.position - transform.position).normalized * (CollectSpeed * Time.deltaTime);
                return;
            }
            _coinsFactory.CollectCoin();
            PoolManager.Despawn(this);
        }
    }
}