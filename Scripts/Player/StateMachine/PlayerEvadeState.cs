using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvadeState : PlayerGroundState
{
    public PlayerEvadeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        //stateMachine.movementSpeedModifier = 0;
        stateMachine.player.status.isinvincible = true;

        if (stateMachine.player.isEvadable)
        {
            stateMachine.player.slowMotionController.onEndSlowMotion += OnEndSlowMotion;

            stateMachine.player.slowMotionController.StartSlowMotion();

            stateMachine.player.animator.speed = 0.5f;
        }
        else
        {
            stateMachine.player.ChangeCameraView(true);

            stateMachine.player.thirdPersonAnimator.SetTrigger(stateMachine.player.animationData.evadeParameterHash);
        }


        if(stateMachine.movementInput == Vector2.zero)
        {
            stateMachine.player.forceReceiver.AddForce(stateMachine.player.thirdPerson.transform.forward *
                stateMachine.player.data.groundedData.evadeStrength);
        }
        else
        {
            stateMachine.player.thirdPersonModel.forward = GetMovementDirection();
            stateMachine.player.forceReceiver.AddForce(GetMovementDirection() *
                stateMachine.player.data.groundedData.evadeStrength);
        }

        base.Enter();
    }

    public void OnEndSlowMotion()
    {
        Debug.Log("EndSlow");

        stateMachine.player.status.isinvincible = false;
        stateMachine.player.slowMotionController.onEndSlowMotion -= OnEndSlowMotion;
        stateMachine.player.animator.speed = 1.0f;

        OnMove();
    }

    public void OnEvadeAnimationEnd()
    {
        stateMachine.player.status.isinvincible = false;

        stateMachine.player.thirdPersonModel.forward = stateMachine.player.thirdPerson.transform.forward;
        stateMachine.player.ChangeCameraView(false);

        OnMove();
    }
}
