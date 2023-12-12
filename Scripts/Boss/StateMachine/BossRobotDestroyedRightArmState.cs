using UnityEngine;

public class BossRobotDestroyedRightArmState : BossRobotBaseState
{
    private float _destroyedTime;

    public BossRobotDestroyedRightArmState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StopNavMechAgent();
        StartAnimation(stateMachine.BossRobot.AnimationData.DestroyedRightArmParameterHash);
        _destroyedTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.BossRobot.AnimationData.DestroyedRightArmParameterHash);
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
