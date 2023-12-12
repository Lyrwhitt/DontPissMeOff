using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int PhaseIndex { get; set; }
    [SerializeField] protected int maxHealth;
    public int health;
    public event Action OnDie;
    public event Action onHealthChange;

    public bool isinvincible = false;

    public bool IsDead => health == 0;

    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void ChangeHealth(int health)
    {
        this.health = health;
        health = Mathf.Clamp(health, 0, maxHealth);

        onHealthChange?.Invoke();
    }

    public virtual void TakeDamage(int damage)
    {
        // 회피중일 시 데미지x
        if (isinvincible)
            return;
        if (health == 0) return;
        health = Mathf.Max(health - damage, 0);

        onHealthChange?.Invoke();

        if (health == 0)
            OnDie?.Invoke();
    }

    public virtual void TakeDamage(int damage, DamageType damageType)
    {
        if (damageType == DamageType.Head)
            damage *= 2;

        if (health == 0) return;
        health = Mathf.Max(health - damage, 0);

        onHealthChange?.Invoke();

        if (health == 0)
            OnDie?.Invoke();
    }

    public virtual float GetPercentageHP()
    {
        return (float)health / maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }
}
