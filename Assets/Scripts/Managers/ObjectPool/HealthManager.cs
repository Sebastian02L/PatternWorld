using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image healthLifeBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float healingActivationTime;
    [SerializeField] float healthRecuperationRate;
    [SerializeField] RedScreenAnimation playerRedScreen;
    float maxHealth;
    float health;
    float timer = 0f;

    public float GetMaxHealth => maxHealth;
    public float GetHealth => health;

    public event Action<int> OnHealthChange;
    public void SetMaxHeahlt(float value)
    {
        maxHealth = value;
        SetHealth(value);
    }
    public void SetHealth(float value)
    {
        health = value;
        UpdateHealthVisuals();
    }
    public void GetDamage(float damage)
    {
        health -= damage;
        UpdateHealthVisuals();
        timer = 0f;
    }

    public void Heal(float healing)
    {
        health += healing;
        UpdateHealthVisuals();
    }

    void UpdateHealthVisuals()
    {
        healthLifeBar.fillAmount = Mathf.Max(0, Mathf.Min(1, health/maxHealth));
        healthText.text = $"{(int)health}/{maxHealth}";
        if (playerRedScreen != null) ShouldActiveRedScreen();
        if(health <= 0) OnHealthChange?.Invoke(0);
        else if(health != maxHealth) OnHealthChange?.Invoke(1);
    }

    void ShouldActiveRedScreen()
    {
        if(health <= 30 && !playerRedScreen.isRedScreenActive)
        {
            playerRedScreen.activateAnim = true;
        }
        else if (health > 30 && playerRedScreen.isRedScreenActive)
        {
            playerRedScreen.activateAnim = true;
        }
    }

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
}
