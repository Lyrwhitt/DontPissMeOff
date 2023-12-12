using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimationEvent : MonoBehaviour
{
    public Player player;

    public void OnEvadeEnd()
    {
        player.stateMachine.evadeState.OnEvadeAnimationEnd();
    }
}
