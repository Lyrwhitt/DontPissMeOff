using UnityEngine;

public class BossRobotAttackCannonState : BossRobotBaseState
{
    public BossRobotAttackCannonState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        TriggerAnimation(stateMachine.BossRobot.AnimationData.AttackRightArmCannonParameterHash);
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        ResetTriggerAnimation(stateMachine.BossRobot.AnimationData.AttackRightArmCannonParameterHash);
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
