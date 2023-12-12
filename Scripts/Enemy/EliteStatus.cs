using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteStatus : Health
{
    private Enemy enemy;

    private void Awake()
    {
        PhaseIndex = 0;
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        // 체력이 200이고, PhaseIndex가 0일 때 1번만 실행
        if (health <= 200 && PhaseIndex == 0)
        {
            PhaseIndex++;
            UpdatePhase();
        }

        // 체력이 100이고, PhaseIndex가 1일 때 1번만 실행
        else if (health <= 100 && PhaseIndex == 1)
        {
            PhaseIndex++;
            UpdatePhase();
        }
    }

    // PhaseIndex 업데이트 후 Animator에 전달하는 메서드
    private void UpdatePhase()
    {
        enemy.Animator.SetInteger("Phase", PhaseIndex);
    }
}
