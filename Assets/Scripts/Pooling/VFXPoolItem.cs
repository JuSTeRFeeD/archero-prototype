using System.Collections;
using UnityEngine;

namespace Pooling
{
    [RequireComponent(typeof(ParticleSystem))]
    public class VFXPoolItem : Poolable
    {
        [SerializeField] private ParticleSystem particle;

        private void OnEnable()
        {
            particle.Play();
            StartCoroutine(nameof(Despawn));
        }

        private IEnumerator Despawn()
        {
            yield return new WaitForSeconds(particle.main.duration);
            PoolManager.Despawn(this);
        }
    }
}
