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

        // ������ �� ����ٲٰ� ���ƿ����� UI���󺹱�
        player.crossHair.OnScope(GunType.SNIPER_RIFLE, false);
    }

    private void OnDisable()
    {
        // �������� ���� �ٲ� �� ó��
        if (isScoped)
        {
            scopeCamera.m_MaxFOV = 60f;
            armCamera.SetActive(true);

            player.crossHair.OnScope(GunType.SNIPER_RIFLE, false);
        }
    }
}