using System.Collections;
using UnityEngine;

public class Shotgun : WeaponBase
{
    protected override void Awake()
    {
        base.Awake();

        leftStrategy = new ShotGunLeft(this);
        rightStrategy = new NothingRight();
        rStrategy = new ShotGunReload(this);
    }
}