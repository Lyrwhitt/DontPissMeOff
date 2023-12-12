using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBase : MonoBehaviour
{
    public Player player;

    public Animator animator;

    [field: Header("Animation")]
    [field: SerializeField] public WeaponAnimationData animationData;

    public Transform bulletSpawn;
    public GunData gunData;
    public CinemachineImpulseSource recoil;

    public bool isReloading = false;
    [field: HideInInspector] public int currentAmmo;
    protected bool _isEquipping = false;
    [field : HideInInspector] public bool isScoped = false;
    protected float _lastShotTime;
    protected float _currentAccuracy;

    public ILeftClick leftStrategy;
    public IRightClick rightStrategy;
    public IRClick rStrategy;

    private IEnumerator _reloadCoroutine;

    public event UnityAction<int> onAmmoChanged;

    protected virtual void Awake()
    {
        animationData.Initialize();
    }

    private void Start()
    {
        currentAmmo = gunData.magazineSize;
        _lastShotTime = -1f;
        _currentAccuracy = gunData.baseAccuracy;

        OnAmmoChanged();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(EquipCoroutine());
    }

    protected virtual void Update()
    {
        // 달리는상태일때 총 못쏘게 추가
        if (_isEquipping || player.stateMachine.isRunning || GameManager.Instance.isPause)  // 만약 무기를 장착 중이라면 총 조작은 할 수 없음
            return;

        SetAccuracy();

        if (gunData.fireMode == FireMode.AUTOMATIC &&
            Input.GetButton("Fire1"))
        {
            if (Time.unscaledTime - _lastShotTime > gunData.fireInterval)
            {
                OnLeftClicked();
                _lastShotTime = Time.unscaledTime;
            }
        }
        else if (gunData.fireMode == FireMode.SINGLE &&
            Input.GetButtonDown("Fire1"))
        {
            if (Time.unscaledTime - _lastShotTime > gunData.fireInterval)
            {
                OnLeftClicked();
                _lastShotTime = Time.unscaledTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRClicked();
        }

        if (Input.GetMouseButtonDown(1)) // 우클릭이 눌렸을 때
        {
            OnRightClicked();
        }
    }
    

    public void OnLeftClicked()
    {
        leftStrategy.OnLeftClicked();
    }

    public void OnRightClicked()
    {
        rightStrategy.OnRightClicked();
    }

    public void OnRClicked()
    {
        rStrategy.OnRClicked();
    }

    public void OnAmmoChanged()
    {
        onAmmoChanged?.Invoke(currentAmmo);
    }


    protected void SetAccuracy()
    {
        if (player.status.isinvincible)
        {
            _currentAccuracy = 1f;

            return;
        }

        if (player.input.playerActions.Movement.ReadValue<Vector2>() != Vector2.zero || 
            Time.unscaledTime - _lastShotTime < 0.5f)
        {
            _currentAccuracy = Mathf.Max(_currentAccuracy - gunData.inaccuracy, 0);
        }
        else
        {
            _currentAccuracy = gunData.baseAccuracy;
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    
    public void Reload(IEnumerator coroutine)
    {
        _reloadCoroutine = coroutine;
        StartCoroutine(_reloadCoroutine);
    }

    public void Throw(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }


    public void OnScope()
    {
        CinemachineFollowZoom scopeCamera = CameraManager.Instance.TryGetFollowZoomCamera();

        if (scopeCamera == null)
            return;

        StartCoroutine(OnScopedCoroutine(scopeCamera));
    }

    public void OnUnScope()
    {
        CinemachineFollowZoom scopeCamera = CameraManager.Instance.TryGetFollowZoomCamera();

        if (scopeCamera == null)
            return;

        StartCoroutine(OnUnscopedCoroutine(scopeCamera));
    }

    
    public void StopReloadCoroutine()
    {
        StopCoroutine(_reloadCoroutine);
    }

    private IEnumerator EquipCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(gunData.equipTime);

        _isEquipping = true;

        yield return waitForSeconds;

        _isEquipping = false;
        isReloading = false;
    }

    private IEnumerator OnScopedCoroutine(CinemachineFollowZoom scopeCamera)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);

        isScoped = true;

        DOTween.To(() => 60f, fov => scopeCamera.m_MaxFOV = fov, 60 / gunData.scopeFactor, 0.15f);

        yield return waitForSeconds;

        player.crossHair.OnScope(gunData.gunType, isScoped);
        player.armCamera.gameObject.SetActive(false);
    }

    private IEnumerator OnUnscopedCoroutine(CinemachineFollowZoom scopeCamera)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);

        isScoped = false;
        player.crossHair.OnScope(gunData.gunType, isScoped);

        DOTween.To(() => 60 / gunData.scopeFactor, fov => scopeCamera.m_MaxFOV = fov, 60f, 0.15f);
        player.armCamera.gameObject.SetActive(true);

        yield return waitForSeconds;
    }

    public void PlayShootSound(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.PISTOL:
                SoundManager.Instance.Play("Gun", SoundManager.Sound.Effect);
                break;
            case GunType.RIFLE:
                SoundManager.Instance.Play("Rifle", SoundManager.Sound.Effect);
                break;
            case GunType.SUB_MACHINE_GUN:
                SoundManager.Instance.Play("Rifle", SoundManager.Sound.Effect);
                break;
            case GunType.SHOTGUN:
                SoundManager.Instance.Play("Shotgun", SoundManager.Sound.Effect);
                break;
            case GunType.SNIPER_RIFLE:
                SoundManager.Instance.Play("SniperRifle", SoundManager.Sound.Effect);
                break;
            case GunType.GRENADE:
                //SoundManager.Instance.Play("SniperRifle", SoundManager.Sound.Effect);
                break;
        }
    }
    public GameObject GetPooledBullet()
    {
        return ObjectPool.Instance.SpawnFromPool("Bullet", bulletSpawn.position, Quaternion.identity);
    }

    public void FireBullet(GameObject bulletObject, Vector3 targetDirection)
    {
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bulletObject.SetActive(true);

        if (bullet != null)
        {
            bullet.damage = gunData.damage;
            bullet.owner = player.gameObject;
            bullet.trailRenderer.Clear();
        }

        bullet.transform.position = bulletSpawn.position;
        bullet.transform.rotation = Quaternion.LookRotation(targetDirection);

        Vector3 inaccuracy = Random.insideUnitSphere * (1f - _currentAccuracy) * 0.05f;
        Vector3 direction = (gunData.gunType == GunType.SHOTGUN) ?
            (targetDirection + new Vector3(inaccuracy.x, inaccuracy.y, 0f)).normalized + Random.insideUnitSphere * gunData.shotgunSpread :
            (targetDirection + new Vector3(inaccuracy.x, inaccuracy.y, 0f)).normalized;

        bullet.SetBullet(gunData.bulletSpeed, direction);
    }
}
