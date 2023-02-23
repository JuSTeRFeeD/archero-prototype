using System.Globalization;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Entity entity;
        [SerializeField] private Image fillBarImage;
        [SerializeField] private TextMeshProUGUI healthAmount;

        private void Start()
        {
            entity.OnHealthChange += UpdateBar;
            UpdateBar(entity);
        }

        private void UpdateBar(Entity e)
        {
            fillBarImage.fillAmount = e.HealthPercent;
            healthAmount.text = e.HealthAmount.ToString(CultureInfo.InvariantCulture);
        }
    }
}
