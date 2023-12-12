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
        float direction = Mathf.Sin(Time.time); // �ð��� ���� �¿�� �����̵��� ��
        if (direction < 0)
        {
            // ������ -1�� ���, �ִϸ��̼��� �ݴ�� ����
            stateMachine.Enemy.Animator.SetFloat("Speed", -1f);
        }
        else
        {
            // ������ 0 �̻��� ���, �ִϸ��̼��� �������� ����
            stateMachine.Enemy.Animator.SetFloat("Speed", 1f);
        }
        Move(direction * stateMachine.Enemy.transform.right);
    }
    private void AdjustWeaponRotation(Vector3 hitpoint)
    {
        // hitpoint�� ���ϴ� �������� ȸ���ϴ� ���ʹϾ� ����
        weapon.transform.rotation = Quaternion.LookRotation(hitpoint - weapon.transform.position);
    }
}
