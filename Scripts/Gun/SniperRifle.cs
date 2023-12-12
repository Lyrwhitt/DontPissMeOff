using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SniperRifle : WeaponBase
{
    public GameObject armCamera;
    public CinemachineFollowZoom scopeCamera;


    protected override void Awake()
    {
        base.Awake();

        leftStrategy = new SniperRifleLeft(this);
        rightStrategy = new Scope(this);
        rStrategy = new Reload(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        isScoped = false;

        // 스코프 중 무기바꾸고 돌아왔을때 UI원상복구
        player.crossHair.OnScope(GunType.SNIPER_RIFLE, false);
    }

    private void OnDisable()
    {
        // 스코프중 무기 바꿀 때 처리
        if (isScoped)
        {
            scopeCamera.m_MaxFOV = 60f;
            armCamera.SetActive(true);

            player.crossHair.OnScope(GunType.SNIPER_RIFLE, false);
        }
    }
}