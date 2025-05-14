using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectPoolMinigame
{
    //Can be used by the player and the enemies
    public class HealthManager : MonoBehaviour
    {
        [Header("Graphical Elements")]
        [SerializeField] Image healthLifeBar;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] RedScreenAnimation playerRedScreen;

        [Header("Pasisve Healing Configuration")]
        [SerializeField] float healingActivationTime;
        [SerializeField] float healthRecuperationRate;

        float maxHealth;
        float health;
        float timer = 0f;

        //Getters
        public float GetMaxHealth => maxHealth;
        public float GetHealth => health;

        //Invoked when the character gets damage, the bool indicates if the character has beeen eliminated
        public event Action<bool> OnGetDamage;

        //Checks if the character needs to heal himself after not receiving damage for some time
        private void Update()
        {
            if (health >= maxHealth) return;
            else
            {
                timer += Time.deltaTime;
                if (timer > healingActivationTime)
                {
                    Heal(healthRecuperationRate * Time.deltaTime);
                }
            }
        }

        //Sets the max health value of the character
        public void SetMaxHeahlt(float value)
        {
            maxHealth = value;
            SetHealth(value);
        }

        //Sets the current health of the character
        public void SetHealth(float value)
        {
            health = value;
            UpdateHealthVisuals();
        }

        //Called when the character gets damage
        public void GetDamage(float damage)
        {
            health -= damage;
            UpdateHealthVisuals();

            if (health < 1) OnGetDamage?.Invoke(true);
            else OnGetDamage?.Invoke(false);
            timer = 0f;
        }

        //Heals the character
        public void Heal(float healing)
        {
            health += healing;
            UpdateHealthVisuals();
        }

        //Updates the UI elemetns related to the healht of the character
        void UpdateHealthVisuals()
        {
            healthLifeBar.fillAmount = Mathf.Max(0, Mathf.Min(1, health / maxHealth));
            healthText.text = $"{(int)health}/{maxHealth}";

            if (playerRedScreen != null) ShouldActiveRedScreen();
        }

        //Checks if the player needs to see the "Red Screen Effect"
        void ShouldActiveRedScreen()
        {
            if (health <= 30 && !playerRedScreen.isRedScreenActive)
            {
                playerRedScreen.activateAnim = true;
            }
            else if (health > 30 && playerRedScreen.isRedScreenActive)
            {
                playerRedScreen.activateAnim = true;
            }
        }
    }
}
