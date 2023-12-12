using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleAttackState : EnemyAttackState
{
    private EnemyGun weapon;
    public EnemyRifleAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
        if (stateMachine.Enemy.Weapon != null)
        {
            weapon = stateMachine.Enemy.Weapon as EnemyGun;
        }
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (normalizedTime >= 1f)
        {         
            if (IsInChaseRange())
            {
                if (stateMachine.Enemy.EnemyHealth.PhaseIndex == 1)
                { 
                    stateMachine.ChangeState(stateMachine.MovePosState);
                    return;
                }
                if (!IsInAttackRange())
                {
                    stateMachine.ChangeState(stateMachine.ChasingState);
                    return;
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
            if (Time.time - lastShootTime >= weapon.gunData.fireInterval)
            {
                weapon.Shoot();
                lastShootTime = Time.time;
            }
           
        }        
    }
}
