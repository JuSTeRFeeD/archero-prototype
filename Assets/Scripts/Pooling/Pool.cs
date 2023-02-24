using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    [DisallowMultipleComponent]
    public class Pool : MonoBehaviour
    {
        public GameObject Prefab { get; private set; }
        public Transform PoolItemsContainer { get; private set; }
        
        public readonly List<Poolable> PooledItems = new(10);

        private bool _isInitialized;
        
        public void Init(GameObject prefab, Transform container)
        {
            if (_isInitialized) return;
            
            Prefab = prefab;
            PoolItemsContainer = container;
            _isInitialized = true;
        }
        
        public Poolable GetFreeItem()
        {
            if (PooledItems.Count > 0)
            {
                var item = PooledItems[0];
                PooledItems.RemoveAt(0);
                return item;
            }
            return InstantiateNewItem();
        }

        private void OnDestroy()
        {
            PoolManager.DestroyPool(this);
        }

        private Poolable InstantiateNewItem()
        {
            var item = Instantiate(Prefab, PoolItemsContainer);

            if (item.TryGetComponent(out Poolable poolableItem))
            {
                poolableItem.InitItem(this, Prefab);
                return poolableItem;
            }
            else
            {
                var i = item.AddComponent<Poolable>();
                i.InitItem(this, Prefab);
                return i;
            }
        } 
        
        public void ExcludeItem(Poolable item)
        {
            if (Prefab != item.Prefab)
            {
#if DEBUG
                Debug.LogError($"Trying to exclude item from other pool (${item.name})");
#endif
                return;
            }
            PooledItems.Remove(item);
        }

        public void IncludeItem(Poolable item)
        {
            if (Prefab != item.Prefab)
            {
#if DEBUG
                Debug.LogError($"Trying to include item from other pool (${item.name})");
#endif
                return;
            }
            PooledItems.Add(item);
        }
    }
}
