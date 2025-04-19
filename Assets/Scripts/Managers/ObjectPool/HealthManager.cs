using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image healthLifeBar;
    float maxHealth;
    float health;

    public float GetMaxHealth => maxHealth;
    public float GetHealth => health;

    public event Action OnHealthChange;
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

        if (!CheckDeath())
        {
            UpdateLifeBar();
        }
    }

    void UpdateLifeBar()
    {
        healthLifeBar.fillAmount = Mathf.Max(0, Mathf.Min(1, health/maxHealth));
        OnHealthChange?.Invoke();
    }

    bool CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
