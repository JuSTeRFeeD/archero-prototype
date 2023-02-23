using Core.Loot;
using Core.Map;
using UnityEngine;

namespace Core.Enemies
{
    public class Enemy : Entity
    {
        private LootCoinsFactory _lootCoinsFactory;
        private Pathfinding _pathfinding;
        
        [SerializeField] private int minDropCoins = 0;
        [SerializeField] private int maxDropCoins = 10;
        [Space]
        [SerializeField] private int damageOnCollide = 10;

        public void Setup(Pathfinding pathfinding, LootCoinsFactory lootCoinsFactory)
        {
            _pathfinding = pathfinding;
            _lootCoinsFactory = lootCoinsFactory;
            
            OnDeath += DropCoins;
        }

        private void DropCoins(Entity e)
        {
            _lootCoinsFactory.SpawnCoins(Random.Range(minDropCoins, maxDropCoins), transform.position);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.CompareTag("Player")) return;
            if (collision.collider.TryGetComponent(out Entity player))
            {
                player.TakeDamage(new DamageData
                {
                    Damage = damageOnCollide
                });
            }
        }
    }
}
