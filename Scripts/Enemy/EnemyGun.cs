using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GunData gunData;

    [SerializeField] private Enemy enemy;
    [SerializeField] private ParticleSystem _particleSystem;
    private float currentAccuracy;

    private void Awake()
    {
        if(bulletSpawn.childCount != 0)
            trajectory = bulletSpawn.GetChild(0).gameObject;

        // 초기 정확도 설정
        currentAccuracy = gunData.baseAccuracy;
        _particleSystem.Stop();
    }

    public void Shoot()
    {
        if (gunData.gunType == GunType.SHOTGUN)
        {
            for (int i = 0; i < gunData.shotgunPellets; i++)
            {
                GameObject bulletObject = ObjectPool.Instance.SpawnFromPool("Bullet", bulletSpawn.position, bulletSpawn.rotation);
                if (bulletObject != null)
                {
                    FireBullet(bulletObject);
                }
            }
        }
        else
        {
            GameObject bulletObject = ObjectPool.Instance.SpawnFromPool("Bullet", bulletSpawn.position, bulletSpawn.rotation);
            if (bulletObject != null)
            {
                FireBullet(bulletObject);
            }
        }
    }

    private void FireBullet(GameObject bulletObject)
    {
        bulletObject.SetActive(true);
        PlayShootSound(gunData.gunType);
        _particleSystem.Play();

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bulletObject.SetActive(true);

        if (bullet != null)
        {
            bullet.damage = gunData.damage;
            bullet.owner = this.gameObject;
        }

        bullet.transform.position = bulletSpawn.position;
        bullet.transform.rotation = bulletSpawn.rotation;
        Vector3 inaccuracy = Random.insideUnitSphere * (1f - currentAccuracy) * 0.05f;
        Vector3 direction = (transform.forward + new Vector3(inaccuracy.x, inaccuracy.y, 0f)).normalized;

        bullet.SetBullet(gunData.bulletSpeed, direction);
    }

    private void PlayShootSound(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.PISTOL:
                SoundManager.Instance.Play("Gun", SoundManager.Sound.Effect);
                break;
            case GunType.RIFLE:
                SoundManager.Instance.Play("Rifle_Shot_1", SoundManager.Sound.Effect);
                break;
            case GunType.SUB_MACHINE_GUN:
                SoundManager.Instance.Play("Rifle", SoundManager.Sound.Effect);
                break;
            case GunType.SHOTGUN:
                SoundManager.Instance.Play("Shotgun", SoundManager.Sound.Effect);
                break;
            case GunType.SNIPER_RIFLE:
                SoundManager.Instance.Play("Sniper_Rifle_Shot_1", SoundManager.Sound.Effect);
                break;
        }
    }
}
