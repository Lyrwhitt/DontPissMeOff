using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovePosState : EnemyBaseState
{
    private int phaseIndex = 0;
    private float invincibilityEndTime;
    public EnemyMovePosState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1f;
        base.Enter();
        phaseIndex = stateMachine.Enemy.EnemyHealth.PhaseIndex;
        stateMachine.Enemy.EnemyHealth.isinvincible = true;
        StartAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
        invincibilityEndTime = Time.time + 2f;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Enemy.EnemyHealth.isinvincible = false;
        StopAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        if(!IsInReturnPosition())
        {
            MoveTo(ReturnToOriginalPosition());
            if (Time.time >= invincibilityEndTime && stateMachine.Enemy.EnemyHealth.isinvincible == true)
            {
                // isinvincible를 false로 변경
                stateMachine.Enemy.EnemyHealth.isinvincible = false;
            }
            return;
        }
        else
        {
            switch(phaseIndex)
            {
                case 1:
                    stateMachine.ChangeState(stateMachine.ProneAttackState);
                    break;
                case 2:
                    stateMachine.ChangeState(stateMachine.WalkCrouchingAttackState);
                    break;
            }
        }
    }
    private bool IsInReturnPosition()
    {
        float playerDistanceSqr = (stateMachine.Enemy.PosData.targetPositions[phaseIndex - 1] - stateMachine.Enemy.transform.position).sqrMagnitude;

        return playerDistanceSqr <= 0.1;
    }
    private Vector3 ReturnToOriginalPosition()
    {
        navMesh.SetDestination(stateMachine.Enemy.PosData.targetPositions[phaseIndex-1]);
        // 현재 위치에서 원래 위치로의 방향 계산
        return navMesh.desiredVelocity.normalized;
    }
}
