using UnityEngine;

public class BossRobotAttackStompState : BossRobotBaseState
{
    public BossRobotAttackStompState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        TriggerAnimation(stateMachine.BossRobot.AnimationData.AttackStompParameterHash);
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Time.time - lastAttackTime >= stateMachine.BossRobot.GetAttackDelayTime())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
