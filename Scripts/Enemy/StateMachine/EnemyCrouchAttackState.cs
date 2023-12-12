using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrouchAttackState : EnemyCrouchIdleState
{
    public EnemyCrouchAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {

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
        //base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (normalizedTime >= 1f)
        {
            stateMachine.ChangeState(stateMachine.CrouchIdleState);
            return;
        }

        if (Time.time - lastShootTime >= weapon.gunData.fireInterval)
        {
            weapon.ShowOffTrajectory();
            weapon.Shoot();
            lastShootTime = Time.time;
        }
    }
}
