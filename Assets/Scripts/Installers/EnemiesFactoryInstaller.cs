using Core.Enemies;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemiesFactoryInstaller : MonoInstaller
    {
        [SerializeField] private EnemiesFactory enemiesFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<EnemiesFactory>().FromInstance(enemiesFactory).AsSingle().NonLazy();
        }
    }
}