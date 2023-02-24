using UnityEngine;

namespace Core.PlayerSkills
{
    public abstract class Skill : ScriptableObject
    {
        public SkillType skillType;
        public Sprite icon;
        [TextArea]
        public string description;
        
    }
}
