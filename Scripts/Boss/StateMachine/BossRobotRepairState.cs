public class BossRobotRepairState : BossRobotBaseState
{
    public BossRobotRepairState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.BossRobot.AnimationData.JumpParameterHash);
        StopNavMechAgent();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.BossRobot.AnimationData.JumpParameterHash);
    }

    public override void Update()
    {
        if (stateMachine.BossRobot.IsFinalPhase())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}