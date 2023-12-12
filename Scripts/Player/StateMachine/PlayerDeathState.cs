using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerGroundState
{
    public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        CameraManager.Instance.ChangeBlendStyle(Cinemachine.CinemachineBlendDefinition.Style.HardOut);
        CameraManager.Instance.ChangeBlendTime(1.3f);
        CameraManager.Instance.ChangeCameraView(stateMachine.player.deathCamera);

        stateMachine.player.thirdPerson.SetActive(true);
        stateMachine.player.firstPerson.SetActive(false);

        stateMachine.movementSpeedModifier = 0f;

        stateMachine.player.thirdPersonAnimator.SetTrigger(stateMachine.player.animationData.deathParameterHash);
    }

    public override void Exit()
    {
        CameraManager.Instance.ChangeBlendStyle(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut);
        CameraManager.Instance.ChangeBlendTime(0.1f);
        CameraManager.Instance.ChangeCameraView(stateMachine.player.firstCamera);

        stateMachine.player.thirdPerson.SetActive(false);
        stateMachine.player.firstPerson.SetActive(true);
    }
}