using UnityEngine;

public class BossRobotAttackMissileState : BossRobotBaseState
{
    public BossRobotAttackMissileState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        TriggerAnimation(stateMachine.BossRobot.AnimationData.AttackShoulderMissileParameterHash);
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        //ResetTriggerAnimation(stateMachine.BossRobot.AnimationData.AttackShoulderMissileParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (IsInStompAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackStompState);
            return;
        }
        else if (Time.time - lastAttackTime >= stateMachine.BossRobot.GetAttackMissileDelayTime())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
