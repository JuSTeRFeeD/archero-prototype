using Core.PlayerSkills;
using UnityEngine;
using Zenject;

namespace Installers
{
    [RequireComponent(typeof(SkillsStore))]
    public class SkillsStoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SkillsStore>().FromInstance(GetComponent<SkillsStore>()).AsSingle();
        }
    }
}