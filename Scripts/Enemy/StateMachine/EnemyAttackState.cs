using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    protected float lastShootTime;
    public EnemyAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0;
        lastShootTime = 0f;
        base.Enter();
        navMesh.enabled = false;
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        navMesh.enabled = true;
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
    }
    public override void Update()
    {
        Rotate(GetTargetDirection());
    }
    private Vector3 GetTargetDirection()
    {
        Vector3 direction = stateMachine.Target.transform.position - stateMachine.Enemy.transform.position;
        return direction.normalized;
    }
}
