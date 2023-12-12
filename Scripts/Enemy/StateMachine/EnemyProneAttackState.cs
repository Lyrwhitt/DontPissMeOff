using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProneAttackState : EnemyAttackState
{
    private EnemyGun weapon;
    public EnemyProneAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
        if (stateMachine.Enemy.Weapon != null)
        {
            weapon = stateMachine.Enemy.Weapon as EnemyGun;
            
        }
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.Enemy.Controller.height = 0.5f;
        stateMachine.Enemy.Controller.center = new Vector3(0f, 0.4f, 0f);
        stateMachine.Enemy.Head.center = new Vector3(0, -1.35f, 0.55f);
        StartAnimation(stateMachine.Enemy.AnimationData.ProneAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Enemy.Controller.height = 2.5f;
        stateMachine.Enemy.Controller.center = new Vector3(0f, 1.3f, 0f);
        stateMachine.Enemy.Head.center = new Vector3(0, 0, 0);
        StopAnimation(stateMachine.Enemy.AnimationData.ProneAttackParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.Enemy.EnemyHealth.PhaseIndex == 2)
        {
            stateMachine.ChangeState(stateMachine.MovePosState);
            return;
        }

        if (IsInAttackRange())
        {
             AdjustWeaponRotation(hitPoint);
            if (Time.time - lastShootTime >= weapon.gunData.fireInterval)
            {
                
                weapon.Shoot();
                lastShootTime = Time.time;
            }
        }
    }
    private void AdjustWeaponRotation(Vector3 hitpoint)
    {
        // hitpoint를 향하는 방향으로 회전하는 쿼터니언 생성
        weapon.transform.rotation = Quaternion.LookRotation(hitpoint - weapon.transform.position);
    }
}
