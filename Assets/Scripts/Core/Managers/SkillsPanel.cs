using System;
using Core.PlayerSkills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Managers
{
    [Serializable]
    public struct SkillItemUI
    {
        public Image icon;
        public TextMeshProUGUI description;
    }
    
    [DisallowMultipleComponent]
    public class SkillsPanel : MonoBehaviour
    {
        [Inject] private SkillsStore _skillsStore;
        [Inject] private GameStats _gameStats;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private Entity _playerEntity;
        
        [SerializeField] private RectTransform skillsPanel;

        public SkillItemUI[] items = new SkillItemUI[3];
        private Skill[] _skills = new Skill[3];
        
        private void Start()
        {
            _playerEntity = _playerMovement.GetComponent<Entity>();
            _playerAttack = _playerMovement.GetComponent<PlayerAttack>();
            
            skillsPanel.gameObject.SetActive(false);
            _gameStats.OnLevelUp += () => SetActivePanel(true);
        }

        private void SetActivePanel(bool value)
        {
            if (value) UpdateItems();
                
            skillsPanel.gameObject.SetActive(value);
            _gameStateManager.SetPause(value);
        }

        private void UpdateItems()
        {
            _skills = _skillsStore.GetRandomSkills();
            for (var i = 0; i < items.Length; i++)
            {
                items[i].icon.sprite = _skills[i].icon;
                items[i].description.text = _skills[i].description;
            }
        }

        public void SelectSkill(int number)
        {
            var selected = _skills[number];
            if (selected is UpgradeStatSkill skill)
            {
                switch (skill.upgradeStatType)
                {
                    case UpgradeStatType.MaxHealth:
                        _playerEntity.IncreaseMaxHealth((int)skill.addValue);
                        break;
                    case UpgradeStatType.CurrentHealth:
                        _playerEntity.Heal((int)skill.addValue);
                        break;
                    case UpgradeStatType.MovementSpeed:
                        _playerMovement.IncreaseMovementSpeed(skill.addValue);
                        break;
                    case UpgradeStatType.AttackDamage:
                        _playerAttack.IncreaseAttackDamage((int)skill.addValue);
                        break;
                    case UpgradeStatType.AttackSpeed:
                        _playerAttack.IncreaseShootSpeed(skill.addValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            Close();
        }

        public void Close()
        {
            SetActivePanel(false);
        }
    }
}