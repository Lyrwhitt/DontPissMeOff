using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : Health
{
    private Player player;
    private DamageIndicator _damageIndicator;

    private float maxStamina = 100f;
    public float stamina;

    private void Awake()
    {
        player = this.GetComponent<Player>();
        _damageIndicator = this.GetComponent<DamageIndicator>();
    }

    protected override void Start()
    {
        SetData(player.data);
    }

    public void SetData(PlayerSO playerData)
    {
        PlayerStatusData data = playerData.statusData;

        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        stamina = maxStamina;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(health >= 0)
        {
            _damageIndicator.Flash();

            //UIManager.Instance.OnPlayerHPChanged();
        }
    }

    public float GetPercentageStamina()
    {
        return stamina / maxStamina;
    }
}
