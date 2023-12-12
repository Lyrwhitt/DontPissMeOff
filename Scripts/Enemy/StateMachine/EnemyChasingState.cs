using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        else if (stateMachine.Enemy.EnemyHealth.PhaseIndex == 1)
        {
            stateMachine.ChangeState(stateMachine.MovePosState);
            return;
        }
        else if (IsInAttackRange())
        {
            if (stateMachine.Enemy.statsChangeType == StatsChangeType.Melee)
            {
                stateMachine.ChangeState(stateMachine.ComboAttackState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.RifleAttackState);
                return;
            }
        }
    }
}
