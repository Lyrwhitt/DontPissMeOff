using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Pistol : WeaponBase
{
    protected override void Awake()
    {
        base.Awake();

        leftStrategy = new BaseShot(this);
        rightStrategy = new NothingRight();
        rStrategy = new Reload(this);
    }
}