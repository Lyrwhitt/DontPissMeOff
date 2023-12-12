using UnityEngine;

public class BossRobotAttackBeamState : BossRobotBaseState
{
    public BossRobotAttackBeamState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        TriggerAnimation(stateMachine.BossRobot.AnimationData.AttackLeftArmBeamParameterHash);
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        ResetTriggerAnimation(stateMachine.BossRobot.AnimationData.AttackLeftArmBeamParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (IsInStompAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackStompState);
            return;
        }
        else if (Time.time - lastAttackTime >= stateMachine.BossRobot.GetAttackDelayTime())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
