using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EventEnemy : MonoBehaviour
{
    private const int CRITICAL_RATE = 30;

    public List<GameObject> linkedEnemy;

    public ParticleSystem particle;

    [Header("Gun")]
    private float currentAccuracy;
    public GunData gunData;
    public Transform bulletSpawn;

    private float lastShootTime;
    private Player player;

    [field: Header("References")]
    [field: SerializeField] private EnemySO _data;

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }

    [field: Header("Health")]
    public Health EnemyHealth;

    [field: HideInInspector] public CharacterController controller { get; private set; }

    [field: Header("Colliders")]
    public SphereCollider headCollider;

    [field: Header("Enemy Type")]
    public StatsChangeType statsChangeType = StatsChangeType.Rifle;


    [Range(0.1f, 1f)]
    public float spreadOffset;

    public bool isActioning = true;
    private bool isTutorial = false;

    [HideInInspector]
    public EventEnemyDirector _enemyDirector;


    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        // 초기 정확도 설정
        currentAccuracy = gunData.baseAccuracy;

        _enemyDirector = this.GetComponent<EventEnemyDirector>();
    }

    private void Start()
    {
        player = GameManager.Instance.player;

        EnemyHealth.OnDie += OnDie;
    }

    private void Update()
    {
        if (isActioning)
            return;

        if (!isTutorial)
        {
            isTutorial = true;

            if (!PlayerPrefs.HasKey("EventModeGuide2"))
            {
                Shoot(PlayRandom(100));

                StartCoroutine(EnemyTutorial());
            }
        }

        if (Time.time - lastShootTime >= gunData.fireInterval)
        {
            if (!player.isHide && player.isEventMode)
            {
                if (statsChangeType == StatsChangeType.Sniper)
                    Shoot(PlayRandom(100));
                else
                    Shoot(PlayRandom(CRITICAL_RATE));
            }
            else
                Shoot(false);
            lastShootTime = Time.time;
        }

        this.transform.rotation = GetLookRotation();
    }

    private IEnumerator EnemyTutorial()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.4f);

        yield return waitForSeconds;

        GameManager.Instance.guideController.StartGuide(Resources.Load<GuideData>("Guide/EventModeGuide2"));
    }

    public Quaternion GetLookRotation()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        return targetRotation;
    }

    public void Shoot(bool isDamagable)
    {
        if (isDamagable)
        {
            particle.transform.position = bulletSpawn.transform.position;
            particle.Play();
        }

        this.Animator.SetTrigger(AnimationData.BaseAttackParameterHash);

        GameObject bulletObject = ObjectPool.Instance.SpawnFromPool("EventBullet", bulletSpawn.position, bulletSpawn.rotation);

        if (bulletObject != null)
        {
            FireBullet(bulletObject, isDamagable);
        }
    }

    private void FireBullet(GameObject bulletObject, bool isDamagable)
    {
        bulletObject.SetActive(true);

        PlayShootSound(gunData.gunType);

        EventBullet bullet = bulletObject.GetComponent<EventBullet>();
        if (bullet != null)
        {
            bullet.damage = _data.Damage;
            bullet.isDamagable = isDamagable;
        }

        bullet.transform.position = bulletSpawn.position;
        bullet.transform.rotation = bulletSpawn.rotation;

        Vector3 inaccuracy = Random.insideUnitSphere * (1f - currentAccuracy) * spreadOffset;
        Vector3 direction = Vector3.zero;

        if (!isDamagable)
        {       
            direction = (transform.forward + new Vector3(inaccuracy.x, inaccuracy.y, inaccuracy.z));
        }
        else
        {
            direction = transform.forward;
        }

        direction.Normalize();
        bullet.SetBullet(gunData.bulletSpeed, direction);
    }

    void OnDie()
    {
        _enemyDirector.StopAction();

        Animator.SetTrigger("Die");
        controller.enabled = false;
        headCollider.enabled = false;
        _enemyDirector.enabled = false;
        enabled = false;

        for(int i = 0; i < linkedEnemy.Count; i++)
        {
            linkedEnemy[i].SetActive(true);
        }
    }

    private bool PlayRandom(int rate)
    {
        int rand = Random.Range(1, 101);

        if (rand <= rate)
        {
            return true;
        }
        else
            return false;
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
