public class BossRobotChasingState : BossRobotBaseState
{
    public BossRobotChasingState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.BossRobot.AnimationData.ChaseParameterHash);
        StartNavMeshAgent();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.BossRobot.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
