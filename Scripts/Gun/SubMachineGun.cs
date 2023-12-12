using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class SubMachineGun : GunBase
{
    public override void Shoot()
    {
        if (_currentAmmo <= 0)
        {
            Debug.Log("No ammo left, reload first!");
            return;
        }

        base.Shoot();

        // 화면 중앙의 조준선을 기준으로 Raycast를 실행하여 조준선이 가리키는 방향을 계산
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        Vector3 targetDirection = transform.forward;
        if (Physics.Raycast(ray, out hit))
        {
            // 조준선이 가리키는 방향을 계산
            targetDirection = (hit.point - bulletSpawn.position).normalized;
        }

        GameObject bulletObject = GetPooledBullet();
        if (bulletObject != null && !_isReloading)
        {
            FireBullet(bulletObject, targetDirection);
        }
    }
}
*/
