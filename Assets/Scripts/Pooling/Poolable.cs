using UnityEngine;

namespace Pooling
{
    [DisallowMultipleComponent]
    public class Poolable : MonoBehaviour, IPoolItem
    {
        public GameObject Prefab { get; private set; }
        public Pool Pool { get; private set; }

        private bool _isInitialized;
        
        public void InitItem(Pool pool, GameObject prefab)
        {
            if (_isInitialized) return;
            
            Prefab = prefab;
            Pool = pool;
            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (Pool != null)
            {
                Pool.ExcludeItem(this);
            }
        }
    }
}
