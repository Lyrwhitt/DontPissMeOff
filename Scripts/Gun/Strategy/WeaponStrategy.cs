using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SniperRifle
public class SniperRifleLeft : ILeftClick
{
    private WeaponBase _weaponBase;
    private GameObject _hitEffect;

    public SniperRifleLeft(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }

    public void OnLeftClicked()
    {
        if (_weaponBase.currentAmmo <= 0 || _weaponBase.isReloading)
        {
            Debug.Log("No ammo left, reload first!");
            return;
        }

        _weaponBase.currentAmmo--;


        _weaponBase.recoil.GenerateImpulse(Camera.main.transform.forward);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        int layerMask = ~((1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            _hitEffect = ObjectPool.Instance.SpawnFromPool("HitEffect", hit.point, hitRotation);

            if (hit.transform.TryGetComponent(out Health health))
            {       
                // �浹�� ����� �±� Ȯ��
                if (hit.collider.CompareTag("Enemy"))
                {
                    Vector3 hitPosition = hit.point;

                    ObjectPool.Instance.SpawnFromPool("Blood", hit.point, hitRotation);
                    if (health.health != 0)
                        GameManager.Instance.damageUI.ShowDamageText((int)_weaponBase.gunData.damage, hit.point);
                }
                health.TakeDamage(_weaponBase.gunData.damage);
            }
        }

        if (_weaponBase.isScoped)
            _weaponBase.OnUnScope();

        _weaponBase.animator.SetTrigger(_weaponBase.animationData.fireParameterHash);
        _weaponBase.PlayShootSound(_weaponBase.gunData.gunType);
        _weaponBase.OnAmmoChanged();
    }
}
#endregion

#region ShotGun
public class ShotGunLeft : ILeftClick
{
    private WeaponBase _weaponBase;

    public ShotGunLeft(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }

    public void OnLeftClicked()
    {
        if (_weaponBase.currentAmmo <= 0)
        {
            Debug.Log("No ammo left, reload first!");
            return;
        }

        _weaponBase.currentAmmo--;


        if (_weaponBase.isReloading) // ������ ���� ��� ���� ���
        {
            _weaponBase.StopReloadCoroutine(); // ����� �ڷ�ƾ�� �ߴ�
            _weaponBase.isReloading = false;
        }


        // ȭ�� �߾��� ���ؼ��� �������� Raycast�� �����Ͽ� ���ؼ��� ����Ű�� ������ ���
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        // "Block", "Ignore", "Trajectory" ���̾� ����
        int layerMask = ~((1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));
        RaycastHit hit;
        Vector3 targetDirection = _weaponBase.transform.forward;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // ���ؼ��� ����Ű�� ������ ���
            targetDirection = (hit.point - _weaponBase.bulletSpawn.position).normalized;
        }

        for (int i = 0; i < _weaponBase.gunData.shotgunPellets; i++)
        {
            GameObject bulletObject = _weaponBase.GetPooledBullet();
            if (bulletObject != null && !_weaponBase.isReloading)
            {
                _weaponBase.FireBullet(bulletObject, targetDirection);
            }
        }

        _weaponBase.animator.SetTrigger(_weaponBase.animationData.fireParameterHash);
        _weaponBase.PlayShootSound(_weaponBase.gunData.gunType);
        _weaponBase.OnAmmoChanged();
    }
}

public class ShotGunReload : IRClick
{
    private WeaponBase _weaponBase;

    public ShotGunReload(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }
    
    public void OnRClicked()
    {
        if (_weaponBase.isReloading) return; // �̹� ������ ���̸� ����

        if (_weaponBase.currentAmmo < _weaponBase.gunData.magazineSize)
        {
            _weaponBase.Reload(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.18f);

        Debug.Log("Reloading...");
        _weaponBase.isReloading = true;

        // ���� ���� �ִϸ��̼�
        _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadStartParameterHash);
        yield return waitForSeconds;


        // ��ź �� - 1 ��ŭ ���� �� �ִϸ��̼� �ݺ�
        while (_weaponBase.currentAmmo < _weaponBase.gunData.magazineSize - 1)
        {
            if (!_weaponBase.isReloading) // ������� ������ ��ҵ� ���
            {
                yield break;
            }

            _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadingParameterHash);

            waitForSeconds = new WaitForSeconds(_weaponBase.gunData.reloadTime);
            yield return waitForSeconds;

            _weaponBase.animator.ResetTrigger(_weaponBase.animationData.reloadingParameterHash);

            if (!_weaponBase.isReloading) // ������� ������ ��ҵ� ���
            {
                yield break;
            }

            _weaponBase.currentAmmo++;
            _weaponBase.OnAmmoChanged();
        }

        // ������ 1�� ������ �� ������ �ִϸ��̼�
        if (_weaponBase.isReloading) // ������� ������ ��ҵ� ��츦 ���
        {
            _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadEndParameterHash);

            waitForSeconds = new WaitForSeconds(2.11f);
            yield return waitForSeconds;

            _weaponBase.animator.ResetTrigger(_weaponBase.animationData.reloadEndParameterHash);

            if (!_weaponBase.isReloading) // ������� ������ ��ҵ� ���
            {
                yield break;
            }

            _weaponBase.currentAmmo++;
        }

        Debug.Log("Reload complete.");
        _weaponBase.isReloading = false;
        _weaponBase.OnAmmoChanged();
    }
}
#endregion

#region GrenadeThrower
public class GrenadeThrowerLeft : MonoBehaviour, ILeftClick
{
    private WeaponBase _weaponBase;
    private GrenadeThrower _grenadeThrower;

    public GrenadeThrowerLeft(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
        _grenadeThrower = _weaponBase.GetComponent<GrenadeThrower>();
    }

    public void OnLeftClicked()
    {
        if (_weaponBase.currentAmmo <= 0)
        {
            Debug.Log("No ammo left, reload first!");
            return;
        }

        _weaponBase.animator.SetTrigger(_weaponBase.animationData.throwParameterHash);

        _weaponBase.gunData.grenadeCount--;
        GameManager.Instance.saveData.grenadeCount = _weaponBase.gunData.grenadeCount;
        _weaponBase.StartCoroutine(_grenadeThrower.ThrowGrenade(_grenadeThrower._currentGrenade));
        _grenadeThrower.Reload();
    }
}
#endregion

public class BaseShot : ILeftClick
{
    private WeaponBase _weaponBase;

    public BaseShot(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }

    public void OnLeftClicked()
    {
        if (_weaponBase.currentAmmo <= 0 || _weaponBase.isReloading)
        {
            Debug.Log("No ammo left, reload first!");
            return;
        }

        _weaponBase.currentAmmo--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        // "Block", "Ignore", "Trajectory" ���̾� ����
        int layerMask = ~((1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));

        RaycastHit hit;
        Vector3 targetDirection = _weaponBase.transform.forward;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            targetDirection = (hit.point - _weaponBase.bulletSpawn.position).normalized;
        }

        GameObject bulletObject = ObjectPool.Instance.SpawnFromPool("Bullet", _weaponBase.bulletSpawn.position, Quaternion.identity);
        
        if (bulletObject != null)
        {
            _weaponBase.FireBullet(bulletObject, targetDirection);
        }

        _weaponBase.animator.SetTrigger(_weaponBase.animationData.fireParameterHash);
        _weaponBase.PlayShootSound(_weaponBase.gunData.gunType);
        _weaponBase.OnAmmoChanged();
    }
}

public class NothingRight : IRightClick
{
    public void OnRightClicked()
    {
    }
}

public class Reload : IRClick
{
    private WeaponBase _weaponBase;

    public Reload(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }

    public void OnRClicked()
    {
        if (!_weaponBase.isReloading && (_weaponBase.currentAmmo < _weaponBase.gunData.magazineSize))
        {
            if (_weaponBase.isScoped)
                _weaponBase.OnUnScope();

            _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadParameterHash);

            _weaponBase.Reload(ReloadCoroutine());
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_weaponBase.gunData.reloadTime);

        Debug.Log("Reloading...");
        _weaponBase.isReloading = true;

        yield return waitForSeconds;

        Debug.Log("Reload complete.");
        _weaponBase.currentAmmo = _weaponBase.gunData.magazineSize;
        _weaponBase.isReloading = false;

        _weaponBase.OnAmmoChanged();
    }
}

public class NothingReload : IRClick
{
    public void OnRClicked()
    {
        Debug.Log("Nothing");
    }
}

public class Scope : IRightClick
{
    private WeaponBase _weaponBase;

    public Scope(WeaponBase weaponBase)
    {
        _weaponBase = weaponBase;
    }

    public void OnRightClicked()
    {
        if (!_weaponBase.isScoped)
        {
            _weaponBase.OnScope();
        }
        else
        {
            _weaponBase.OnUnScope();
        }
    }
}