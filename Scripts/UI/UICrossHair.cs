using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrossHair : MonoBehaviour
{
    private GameUIController _gameUIController;

    private GameObject currentCrossHair;
    public GameObject pistol;
    public GameObject shotGun;
    public GameObject rifle;
    public GameObject sniperRifle;
    public GameObject subMachineGun;

    [Header("Sniper Rifle")]
    public GameObject sniperNormal;
    public GameObject sniperScope;

    private void Awake()
    {
        _gameUIController = GetComponentInParent<GameUIController>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        ChangeCrossHair(_gameUIController.player.currentWeapon.gunData.gunType);
    }

    public void ChangeCrossHair(GunType gunType)
    {
        currentCrossHair?.SetActive(false);

        GameObject switchCrossHair;

        switch (gunType)
        {
            case GunType.PISTOL:
                switchCrossHair = pistol;
                break;
            case GunType.SHOTGUN:
                switchCrossHair = shotGun;
                break;
            case GunType.SNIPER_RIFLE:
                switchCrossHair = sniperRifle;
                break;
            case GunType.SUB_MACHINE_GUN:
                switchCrossHair = subMachineGun;
                break;
            case GunType.RIFLE:
                switchCrossHair = rifle;
                break;
            default:
                switchCrossHair = pistol;
                break;
        }

        currentCrossHair = switchCrossHair;
        switchCrossHair.SetActive(true);
    }

    public void OnScope(GunType gunType, bool isScope)
    {
        switch (gunType)
        {
            case GunType.SNIPER_RIFLE:
                sniperNormal.SetActive(!isScope);
                sniperScope.SetActive(isScope);
                break;
        }

        _gameUIController.playerStatusUI.gameObject.SetActive(!isScope);
    }
}
