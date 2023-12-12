using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrouchIdleState : EnemyAttackState
{
    protected EnemyGun weapon;
    protected const float SHOW_TRAJECTORY_TIME = 0.1f;
    public EnemyCrouchIdleState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
        if (stateMachine.Enemy.Weapon != null)
        {
            weapon = stateMachine.Enemy.Weapon as EnemyGun;
        }
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.CrouchParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.CrouchParameterHash);
    }
    public override void Update()
    {
        base.Update();
        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (normalizedTime >= 1f)
        {
            if (IsInChaseRange())
            {
                if (IsInAttackRange())
                {
                    stateMachine.ChangeState(stateMachine.CrouchAttackState);
                    return;
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
        }

        else if (normalizedTime >= 1f - SHOW_TRAJECTORY_TIME)
        {
            if (IsInChaseRange() && IsInAttackRange())
            {
                weapon.ShowOnTrajectory();
            }
            else
            {
                weapon.ShowOffTrajectory();
            }
        }
    }

}
