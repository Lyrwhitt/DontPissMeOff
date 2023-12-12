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
                // 충돌한 대상의 태그 확인
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


        if (_weaponBase.isReloading) // 재장전 중인 경우 장전 취소
        {
            _weaponBase.StopReloadCoroutine(); // 저장된 코루틴을 중단
            _weaponBase.isReloading = false;
        }


        // 화면 중앙의 조준선을 기준으로 Raycast를 실행하여 조준선이 가리키는 방향을 계산
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        // "Block", "Ignore", "Trajectory" 레이어 제외
        int layerMask = ~((1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));
        RaycastHit hit;
        Vector3 targetDirection = _weaponBase.transform.forward;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // 조준선이 가리키는 방향을 계산
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
        if (_weaponBase.isReloading) return; // 이미 재장전 중이면 무시

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

        // 장전 시작 애니메이션
        _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadStartParameterHash);
        yield return waitForSeconds;


        // 장탄 수 - 1 만큼 장전 중 애니메이션 반복
        while (_weaponBase.currentAmmo < _weaponBase.gunData.magazineSize - 1)
        {
            if (!_weaponBase.isReloading) // 사격으로 장전이 취소된 경우
            {
                yield break;
            }

            _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadingParameterHash);

            waitForSeconds = new WaitForSeconds(_weaponBase.gunData.reloadTime);
            yield return waitForSeconds;

            _weaponBase.animator.ResetTrigger(_weaponBase.animationData.reloadingParameterHash);

            if (!_weaponBase.isReloading) // 사격으로 장전이 취소된 경우
            {
                yield break;
            }

            _weaponBase.currentAmmo++;
            _weaponBase.OnAmmoChanged();
        }

        // 마지막 1발 장전할 때 마무리 애니메이션
        if (_weaponBase.isReloading) // 사격으로 장전이 취소된 경우를 대비
        {
            _weaponBase.animator.SetTrigger(_weaponBase.animationData.reloadEndParameterHash);

            waitForSeconds = new WaitForSeconds(2.11f);
            yield return waitForSeconds;

            _weaponBase.animator.ResetTrigger(_weaponBase.animationData.reloadEndParameterHash);

            if (!_weaponBase.isReloading) // 사격으로 장전이 취소된 경우
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
        // "Block", "Ignore", "Trajectory" 레이어 제외
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