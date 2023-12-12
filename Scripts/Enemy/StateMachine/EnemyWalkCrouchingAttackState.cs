using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkCrouchingAttackState : EnemyAttackState
{
    private EnemyGun weapon;
    private bool hasAdjustedRotation = false;
    public EnemyWalkCrouchingAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
        if (stateMachine.Enemy.Weapon != null)
        {
            weapon = stateMachine.Enemy.Weapon as EnemyGun;
        }
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = 0.4f;
        StartAnimation(stateMachine.Enemy.AnimationData.CrouchingAttackParameterHash);
        stateMachine.Enemy.Head.center = new Vector3(0.2f, -0.45f, 0.1f);
        hasAdjustedRotation = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.CrouchingAttackParameterHash);
        stateMachine.Enemy.Head.center = new Vector3(0, 0, 0);
        hasAdjustedRotation = false;
    }
    public override void Update()
    {
        base.Update();
        MoveLeftAndRight();
        if (IsInAttackRange())
        {
            if (!hasAdjustedRotation)
            {
                AdjustWeaponRotation(hitPoint);
                hasAdjustedRotation = true;
            }
            if (Time.time - lastShootTime >= weapon.gunData.fireInterval)
            {
                weapon.Shoot();
                lastShootTime = Time.time;
            }
        }
    }
    private void MoveLeftAndRight()
    {
        float direction = Mathf.Sin(Time.time); // 시간에 따라 좌우로 움직이도록 함
        if (direction < 0)
        {
            // 방향이 -1인 경우, 애니메이션을 반대로 실행
            stateMachine.Enemy.Animator.SetFloat("Speed", -1f);
        }
        else
        {
            // 방향이 0 이상인 경우, 애니메이션을 정상으로 실행
            stateMachine.Enemy.Animator.SetFloat("Speed", 1f);
        }
        Move(direction * stateMachine.Enemy.transform.right);
    }
    private void AdjustWeaponRotation(Vector3 hitpoint)
    {
        // hitpoint를 향하는 방향으로 회전하는 쿼터니언 생성
        weapon.transform.rotation = Quaternion.LookRotation(hitpoint - weapon.transform.position);
    }
}
