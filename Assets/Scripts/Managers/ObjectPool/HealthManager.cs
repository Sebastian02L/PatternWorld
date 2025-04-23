using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image healthLifeBar;
    float maxHealth;
    public float health;

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
        UpdateLifeBar();
    }
    public void GetDamage(float damage)
    {
        health -= damage;
        UpdateLifeBar();
        
    }

    void UpdateLifeBar()
    {
        healthLifeBar.fillAmount = Mathf.Max(0, Mathf.Min(1, health/maxHealth));
        if(health <= 0) OnHealthChange?.Invoke(0);
        else OnHealthChange?.Invoke(1);
    }
}
