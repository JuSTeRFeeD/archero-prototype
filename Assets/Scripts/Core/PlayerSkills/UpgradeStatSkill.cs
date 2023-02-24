using UnityEngine;

namespace Core.PlayerSkills
{
    [CreateAssetMenu(menuName = "Skills/upgrade")]
    public class UpgradeStatSkill : Skill
    {
        public UpgradeStatType upgradeStatType;
        public float addValue;
    }
}