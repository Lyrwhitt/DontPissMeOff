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

        // ü���� 200�̰�, PhaseIndex�� 0�� �� 1���� ����
        if (health <= 200 && PhaseIndex == 0)
        {
            PhaseIndex++;
            UpdatePhase();
        }

        // ü���� 100�̰�, PhaseIndex�� 1�� �� 1���� ����
        else if (health <= 100 && PhaseIndex == 1)
        {
            PhaseIndex++;
            UpdatePhase();
        }
    }

    // PhaseIndex ������Ʈ �� Animator�� �����ϴ� �޼���
    private void UpdatePhase()
    {
        enemy.Animator.SetInteger("Phase", PhaseIndex);
    }
}
