using Core.Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameStateManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameStateManager gameStateManager;
        
        public override void InstallBindings()
        {
            Container.Bind<GameStateManager>().FromInstance(gameStateManager).AsSingle();
        }
    }
}