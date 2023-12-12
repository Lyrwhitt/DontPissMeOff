using System.Collections;
using UnityEngine;

public class GrenadeThrower : WeaponBase
{
    public GameObject grenadePrefab;
    public float throwForce = 10f; // ����ź ��ô��
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
            //UIWeapon leftWeapon = uiSelectGun.GetLeftWeapon(); // ���� ���⸦ �����ɴϴ�.
            uiSelectGun.ChangeWeapon(-1); // ���� ����� �����մϴ�.

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

        // ����ź ��� ��� �� ������Ʈ ��Ȱ��ȭ �� ���� �ڵ� ��ü
        if (gunData.grenadeCount == 0)
        {
            //UIWeapon leftWeapon = uiSelectGun.GetLeftWeapon(); // ���� ���⸦ �����ɴϴ�.
            uiSelectGun.ChangeWeapon(-1); // ���� ����� �����մϴ�.

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