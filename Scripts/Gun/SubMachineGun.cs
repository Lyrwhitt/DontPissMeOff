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

        // ȭ�� �߾��� ���ؼ��� �������� Raycast�� �����Ͽ� ���ؼ��� ����Ű�� ������ ���
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        Vector3 targetDirection = transform.forward;
        if (Physics.Raycast(ray, out hit))
        {
            // ���ؼ��� ����Ű�� ������ ���
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
