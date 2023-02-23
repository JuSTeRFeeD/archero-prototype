using Core.Managers;
using Zenject;

namespace Installers
{
    public class GameStatsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStats>().FromInstance(new GameStats()).AsSingle();
        }
    }
}