using Core.Loot;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LootCoinsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private LootCoinsFactory lootCoinsFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<LootCoinsFactory>().FromInstance(lootCoinsFactory).AsSingle();
        }
    }
}