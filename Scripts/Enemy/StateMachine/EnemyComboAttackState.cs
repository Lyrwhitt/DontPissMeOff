using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComboAttackState : EnemyAttackState
{
    private bool alreadyApplyCombo;
    private bool alreadyAppliedDealing;

    ComboAttackInfoData comboAttackInfoData;

    public EnemyComboAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);

        alreadyApplyCombo = false;
        alreadyAppliedDealing = false;

        int comboIndex = stateMachine.ComboIndex;
        comboAttackInfoData = stateMachine.Enemy.Data.AttakData.GetAttackInfo(comboIndex);
        stateMachine.Enemy.Animator.SetInteger("Combo", comboIndex);

        if(comboIndex == 1)
            stateMachine.Enemy.Weapon.ShowOnTrajectory();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);

        if (!alreadyApplyCombo)
            stateMachine.ComboIndex = 0;
    }
    private void TryComboAttack()
    {
        if (alreadyApplyCombo) return;

        if (comboAttackInfoData.ComboStateIndex == -1) return;

        alreadyApplyCombo = true;
    }
    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= comboAttackInfoData.ComboTransitionTime)
                TryComboAttack();
            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Enemy.Data.Dealing_Start_TransitionTime)
            {
                SoundManager.Instance.Play(comboAttackInfoData.AttackSound,SoundManager.Sound.Effect);
                stateMachine.Enemy.Weapon.SetAttack(stateMachine.Enemy.Data.Damage);
                stateMachine.Enemy.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && normalizedTime >= stateMachine.Enemy.Data.Dealing_End_TransitionTime)
            {
                stateMachine.Enemy.Weapon.gameObject.SetActive(false);
                stateMachine.Enemy.Weapon.ShowOffTrajectory();
            }
        }
        else
        {
            if (alreadyApplyCombo)
            {
                if (IsInAttackRange())
                {
                    stateMachine.ComboIndex = comboAttackInfoData.ComboStateIndex;
                    stateMachine.ChangeState(stateMachine.ComboAttackState);
                    return;
                }
                else
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
        }
    }
}
