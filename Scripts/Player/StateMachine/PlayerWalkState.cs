using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.movementSpeedModifier = groundData.walkSpeedModifier;
        base.Enter();

        SetAnimationBool(stateMachine.player.animationData.walkParameterHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SetAnimationBool(stateMachine.player.animationData.walkParameterHash, false);
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.runState);
    }
}
