using Core;
using Core.Map;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MapGridInstaller : MonoInstaller
    {
        [SerializeField] private MapGrid mapGrid;
        public override void InstallBindings()
        {
            Container.Bind<MapGrid>().FromInstance(mapGrid).AsSingle();
        }
    }
}