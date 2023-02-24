using UnityEngine;

namespace Core.PlayerSkills
{
    [DisallowMultipleComponent]
    public class SkillsStore : MonoBehaviour
    {
        private Skill[] _skills;
        
        private readonly Skill[] _randomSkills = new Skill[3];
        
        private void Start()
        {
            var upgrades = Resources.LoadAll<Skill>("Skills/Upgrades/");
            _skills = upgrades;
        }

        public Skill[] GetRandomSkills()
        {
            for (var i = 0; i < _randomSkills.Length; i++)
            {
                _randomSkills[i] = _skills[Random.Range(0, _skills.Length)];
            }
            return _randomSkills;
        }
    }
}
