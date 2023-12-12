using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        navMesh.isStopped = true;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        navMesh.isStopped = false;
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Idle");
        if (normalizedTime >= 1f)
        {
            if (stateMachine.Enemy.statsChangeType != StatsChangeType.Sniper2)
            {
                stateMachine.ChangeState(stateMachine.RandowWalkState);
                return;
            }
        }
        if (IsInChaseRange())
        {
            switch(stateMachine.Enemy.statsChangeType)
            {
                case StatsChangeType.Rifle: case StatsChangeType.Melee: case StatsChangeType.Elite:
                    stateMachine.ChangeState(stateMachine.ChasingState);
                    break;
                case StatsChangeType.Sniper: case StatsChangeType.Sniper2:
                    stateMachine.ChangeState(stateMachine.CrouchIdleState);
                    break;
            }
            return;
        }
        if(stateMachine.Enemy.EnemyHealth.PhaseIndex == 1) {
            stateMachine.ChangeState(stateMachine.MovePosState);
        }
    }
}
