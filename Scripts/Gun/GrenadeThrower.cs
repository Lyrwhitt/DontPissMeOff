using System.Collections;
using UnityEngine;

public class GrenadeThrower : WeaponBase
{
    public GameObject grenadePrefab;
    public float throwForce = 10f; // 수류탄 투척력
    public GameObject _currentGrenade;
    public UISelectGun uiSelectGun;
    public UIWeapon grenadeUI;

    protected override void Awake()
    {
        base.Awake();

        leftStrategy = new GrenadeThrowerLeft(this);
        rightStrategy = new NothingRight();
        rStrategy = new NothingReload();
    }

    protected override void Update()
    {
        base.Update();
        player.statusUI.UpdateBulletState(gunData.grenadeCount);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_currentGrenade == null) SpawnGrenade();
        if (gunData.grenadeCount == 0)
        {
            //UIWeapon leftWeapon = uiSelectGun.GetLeftWeapon(); // 왼쪽 무기를 가져옵니다.
            uiSelectGun.ChangeWeapon(-1); // 왼쪽 무기로 변경합니다.

            grenadeUI.toggle.isOn = false;
            grenadeUI.isUnlocked = false;
            grenadeUI.iconImg.enabled = false;
            gameObject.SetActive(false);
            uiSelectGun.SetChangedWeapon();
        }
    }

    private void SpawnGrenade()
    {
        _currentGrenade = Instantiate(grenadePrefab, bulletSpawn.position, bulletSpawn.rotation, bulletSpawn.transform);
        _currentGrenade.transform.localScale = Vector3.one;
        _currentGrenade.GetComponent<Rigidbody>().useGravity = false;
        _currentGrenade.GetComponent<Collider>().isTrigger = true;
    }

    public IEnumerator ThrowGrenade(GameObject grenade)
    {
        yield return new WaitForSeconds(1.1f);

        grenade.GetComponent<Grenade>().isThorow = true;
        grenade.transform.SetParent(null);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // Set the initial velocity of the grenade
        Vector3 velocity = transform.forward * throwForce;
        rb.useGravity = true;
        rb.velocity = velocity;
        grenade.GetComponent<Collider>().isTrigger = false;

        yield return new WaitForSeconds(0.6f);
        SpawnGrenade();

        // 수류탄 모두 사용 시 오브젝트 비활성화 및 무기 자동 교체
        if (gunData.grenadeCount == 0)
        {
            //UIWeapon leftWeapon = uiSelectGun.GetLeftWeapon(); // 왼쪽 무기를 가져옵니다.
            uiSelectGun.ChangeWeapon(-1); // 왼쪽 무기로 변경합니다.

            grenadeUI.toggle.isOn = false;
            grenadeUI.isUnlocked = false;
            grenadeUI.iconImg.enabled = false;
            gameObject.SetActive(false);
            uiSelectGun.SetChangedWeapon();
        }
    }

    public void Reload()
    {
        if (!isReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    protected IEnumerator ReloadCoroutine()
    {
        Debug.Log("Reloading...");
        isReloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        Debug.Log("Reload complete.");
        currentAmmo = gunData.magazineSize;
        isReloading = false;
    }
}