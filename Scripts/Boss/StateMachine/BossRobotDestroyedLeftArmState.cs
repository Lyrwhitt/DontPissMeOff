using UnityEngine;

public class BossRobotDestroyedLeftArmState : BossRobotBaseState
{
    private float _destroyedTime;

    public BossRobotDestroyedLeftArmState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StopNavMechAgent();
        StartAnimation(stateMachine.BossRobot.AnimationData.DestroyedLeftArmParameterHash);
        _destroyedTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.BossRobot.AnimationData.DestroyedLeftArmParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (Time.time - _destroyedTime > 2f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
