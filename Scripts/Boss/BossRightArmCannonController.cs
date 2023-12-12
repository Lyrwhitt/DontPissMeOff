using System.Collections;
using UnityEngine;

public class BossRightArmCannonController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private GameObject _bulletCartridgePrefab;
    [SerializeField] private GameObject _bulletProjectilePrefab;
    [SerializeField] private GameObject _bulletShellObj;
    [SerializeField] private GameObject _bulletSpawnObj;

    private int _bulletCount = 5;
    private float _bulletSpeed = 50f;
    private int _bulletDelay = 5;
    private GameObject[] _bulletCartridgeObjs;
    private GameObject[] _bulletProjectileObjs;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _bulletCartridgeObjs = new GameObject[_bulletCount];
        _bulletProjectileObjs = new GameObject[_bulletCount];

        for (int i = 0; i < _bulletCount; i++)
        {
            _bulletCartridgeObjs[i] = Instantiate(_bulletCartridgePrefab, _bulletSpawnObj.transform);
            _bulletProjectileObjs[i] = Instantiate(_bulletProjectilePrefab, _bulletSpawnObj.transform);
            _bulletCartridgeObjs[i].SetActive(false);
            _bulletProjectileObjs[i].SetActive(false);
        }
    }

    public void Fire()
    {
        ResetBullet();
        ResetSpawn();

        StartCoroutine(FireCO());
    }

    IEnumerator FireCO()
    {
        int count = 0;
        Vector3 bulletShellPos = Vector3.zero;

        while (count < _bulletCount)
        {
            bulletShellPos = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                _bulletShellObj.transform.localPosition = bulletShellPos;
                bulletShellPos.z += 0.25f;
                yield return null;
            }
            yield return null;

            StartCoroutine(FireBulletCO(count));

            count += 1;
        }
    }

    IEnumerator FireBulletCO(int index)
    {
        float fireTime = Time.time;
        Vector3 playerPos = _playerTransform.position;
        _bulletProjectileObjs[index].SetActive(true);
        Vector3 bulletPos = _bulletProjectileObjs[index].transform.position;
        Quaternion bulletQuaternion = _bulletProjectileObjs[index].transform.rotation;

        while (true)
        {
            bulletPos += _bulletProjectileObjs[index].transform.forward * _bulletSpeed * Time.deltaTime;
            _bulletProjectileObjs[index].transform.position = bulletPos;
            _bulletProjectileObjs[index].transform.rotation = bulletQuaternion;

            for(int i = 0; i < _bulletDelay; i++)
                yield return null;

            if (_bulletProjectileObjs[index].transform.position.y < -1f || Time.time - fireTime > 5f)
            {
                _bulletProjectileObjs[index].SetActive(false);
                break;
            }
        }
    }

    private void ResetBullet()
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            _bulletProjectileObjs[i].transform.localPosition = Vector3.zero;
            _bulletProjectileObjs[i].transform.localRotation = Quaternion.identity;
        }
    }

    private void ResetSpawn()
    {
        _bulletShellObj.transform.localPosition = Vector3.zero;
        _bulletSpawnObj.transform.position = _bulletShellObj.transform.position;
        _bulletSpawnObj.transform.rotation = _bulletShellObj.transform.rotation;
    }
}
