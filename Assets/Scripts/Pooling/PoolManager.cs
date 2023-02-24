using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pooling
{
    public static class PoolManager
    {
        private static readonly List<Pool> Pools = new (64);
        
        private static Pool CreateNewPool(GameObject gameObject)
        {
            var poolContainer = new GameObject($"Pool {gameObject.name}");
            var newPool = poolContainer.AddComponent<Pool>();
            
            newPool.Init(gameObject, poolContainer.transform);
            Pools.Add(newPool);

            return newPool;
        }

        public static Pool GetPoolByPrefab(GameObject prefab)
        {
            foreach (var pool in Pools.Where(i => i.Prefab == prefab))
            {
                return pool;
            }

            return CreateNewPool(prefab);
        }

        public static GameObject Spawn(GameObject toSpawn, Vector3 position = default, Quaternion rotation = default,
            bool worldPositionStays = false)
        {
            return SpawnItem(toSpawn, position, rotation, null, worldPositionStays);
        }
        
        public static T Spawn<T>(T toSpawn, Vector3 position = default, Quaternion rotation = default,
            bool worldPositionStays = false) where T : Component 
        {
            return SpawnItem(toSpawn.gameObject, position, rotation, null, worldPositionStays).GetComponent<T>();
        }
        
        public static void Despawn(GameObject toDespawn)
        {
            DespawnItem(toDespawn);
        }
        
        public static void Despawn(Component toDespawn)
        {
            DespawnItem(toDespawn.gameObject);
        }

        private static GameObject SpawnItem(GameObject toSpawn, Vector3 position, Quaternion rotation,
            Transform parent, bool worldPositionStays)
        {
            var pool = GetPoolByPrefab(toSpawn);
            var newItem = pool.GetFreeItem();
            
            SetupTransform(newItem.transform, position, rotation, parent, worldPositionStays);
            newItem.gameObject.SetActive(true);

            return newItem.gameObject;
        } 
        
        private static void DespawnItem(GameObject toDespawn)
        {
            if (toDespawn.TryGetComponent(out Poolable poolable))
            {
                var pool = poolable.Pool;
                
                toDespawn.transform.SetParent(pool.PoolItemsContainer);
                toDespawn.SetActive(false);

                pool.IncludeItem(poolable);
            }
            else
            {
#if DEBUG
                Debug.LogError($"{toDespawn.name} was not spawned by PoolManager!");
#endif
                Object.Destroy(toDespawn);
            }
        } 
        
        
        private static void SetupTransform(Transform transform, Vector3 position, Quaternion rotation, 
            Transform parent = null, bool worldPositionStays = false)
        {
            transform.SetParent(parent, worldPositionStays);
            transform.SetPositionAndRotation(position, rotation);
        }

        public static void DestroyPool(Pool pool)
        {
            foreach (var i in pool.PooledItems)
            {
                Object.Destroy(i.gameObject);
            }
            Object.Destroy(pool.gameObject);
            Pools.Remove(pool);
        }
    }
}
