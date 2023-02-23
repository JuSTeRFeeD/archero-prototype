using Core;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerReferenceInstaller : MonoInstaller
    {
        [SerializeField] private PlayerMovement playerMovement;

        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(playerMovement).AsSingle().NonLazy();
        }
    }
}