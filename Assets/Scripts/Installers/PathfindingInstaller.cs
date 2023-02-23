using Core.Map;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PathfindingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Pathfinding>().FromInstance(new Pathfinding()).AsSingle();
        }
    }
}