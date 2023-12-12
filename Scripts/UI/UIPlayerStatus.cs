using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    private GameUIController _gameUIController;

    public Slider hpBar;
    public TextMeshProUGUI hpText;

    public Image currentWeaponImg;
    public TextMeshProUGUI bulletCountText;

    private void Awake()
    {
        _gameUIController = GetComponentInParent<GameUIController>();
    }

    private void Start()
    {
        UpdateHP();

        _gameUIController.player.status.onHealthChange += UpdateHP;
    }

    public void UpdateHP()
    {
        hpBar.value = _gameUIController.player.status.GetPercentageHP();
        hpText.SetText(_gameUIController.player.status.health.ToString());
    }

    public void UpdateBulletState(int ammo)
    {
        bulletCountText.SetText(ammo.ToString());
    }

    public void ChangeWeaponImg(GunType gunType)
    {
        string path = $"UI/WeaponIcon/";

        switch(gunType)
        {
            case GunType.PISTOL:
                path = string.Concat(path, "Gun");
                break;
            case GunType.SHOTGUN:
                path = string.Concat(path, "ShotGun");
                break;
            case GunType.SNIPER_RIFLE:
                path = string.Concat(path, "SniperRifle");
                break;
            case GunType.SUB_MACHINE_GUN:
                path = string.Concat(path, "SubMachineGun");
                break;
            case GunType.RIFLE:
                path = string.Concat(path, "Rifle");
                break;
            case GunType.GRENADE:
                path = string.Concat(path, "Grenade");
                break;
        }

        currentWeaponImg.sprite = Resources.Load<Sprite>(path);
    }
}
