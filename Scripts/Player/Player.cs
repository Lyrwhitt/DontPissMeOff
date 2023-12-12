using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isEvadable = false;

    [Header("In EventMode")]
    public bool isEventMode = false;
    public bool isHide = false;
    [field: HideInInspector] public EventScenario currentEvent;

    public GameObject firstPerson;
    public GameObject thirdPerson;
    public Transform thirdPersonModel;
    public CinemachineVirtualCamera firstCamera;
    public CinemachineVirtualCamera thirdCamera;
    public CinemachineVirtualCamera deathCamera;
    public Camera armCamera;

    public Animator thirdPersonAnimator;

    [HideInInspector]
    public PlayerStateMachine stateMachine;

    [field: Header("UI")]
    public UICrossHair crossHair;
    public UIPlayerStatus statusUI;

    [field: Header("Data")]
    [field: SerializeField] public PlayerSO data;

    [field: Header("Animation")]
    [field: SerializeField] public PlayerAnimationData animationData;

    [field: Header("Weapon")]
    [field: HideInInspector] public WeaponBase currentWeapon;
    [field: SerializeField] private Pistol _pistol;
    [field: SerializeField] private SniperRifle _sniperRifle;
    [field: SerializeField] private Rifle _rifle;
    [field: SerializeField] private Shotgun _shotgun;
    [field: SerializeField] private GrenadeThrower _grenadeThrower;


    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerInput input;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public ForceReceiver forceReceiver;
    [HideInInspector]
    public PlayerStatus status;
    [HideInInspector]
    public SlowMotion slowMotionController;


    private void Awake()
    {
        animationData.Initialize();

        AddObserverEvent();
        InitWeaponSet();

        animator = currentWeapon.animator;
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        forceReceiver = GetComponent<ForceReceiver>();
        status = GetComponent<PlayerStatus>();
        slowMotionController = GetComponent<SlowMotion>();

        stateMachine = new PlayerStateMachine(this);

        GameManager.Instance.onPause += OnPause;
        GameManager.Instance.onResume += OnResume;
        GameManager.Instance.onRetry += OnRetry;
    }

    private void Start()
    {
        CameraManager.Instance.ChangeCameraView(firstCamera, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stateMachine.ChangeState(stateMachine.idleState);

        status.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void ChangeWeapon(GunType gunType)
    {
        WeaponBase changedWeapon = currentWeapon;

        switch (gunType)
        {
            case GunType.PISTOL:
                changedWeapon = _pistol;
                break;
            case GunType.SHOTGUN:
                changedWeapon = _shotgun;
                break;
            case GunType.RIFLE:
                changedWeapon = _rifle;
                break;
            case GunType.SNIPER_RIFLE:
                changedWeapon = _sniperRifle;
                break;
            case GunType.SUB_MACHINE_GUN:
                //changedWeapon = _submachineGun;
                break;
            case GunType.GRENADE:
                changedWeapon = _grenadeThrower;
                break;
        }

        if (changedWeapon == currentWeapon)
            return;

        currentWeapon.gameObject.SetActive(false);
        changedWeapon.gameObject.SetActive(true);

        currentWeapon = changedWeapon;
        animator = currentWeapon.animator;

        SetParameters();
    }

    // 현재무기의 에니메이터 파라미터 정보를 변경될 애니메이터에 그대로 적용
    public void SetParameters()
    {
        foreach(var parameter in animationData.boolParameters)
        {
            currentWeapon.animator.SetBool(parameter.Key, parameter.Value);
        }
    }

    public void SetCurrentEventScenario(EventScenario eventScenario)
    {
        this.currentEvent = eventScenario;
    }

    private void OnRetry()
    {
        SaveData savedData = GameManager.Instance.LoadLastData();

        status.ChangeHealth(savedData.playerHP);
        stateMachine.ChangeState(stateMachine.idleState);

        DOTween.Clear();

        CameraManager.Instance.cameraFilter.weight = 0f;
    }

    private void OnDie()
    {
        stateMachine.ChangeState(stateMachine.deathState);

        DOTween.To(() => 0f, weight => CameraManager.Instance.cameraFilter.weight = weight, 1f, 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
    private void AddObserverEvent()
    {
        _pistol.onAmmoChanged += statusUI.UpdateBulletState;
        _shotgun.onAmmoChanged += statusUI.UpdateBulletState;
        _sniperRifle.onAmmoChanged += statusUI.UpdateBulletState;
        _rifle.onAmmoChanged += statusUI.UpdateBulletState;
        _grenadeThrower.onAmmoChanged += statusUI.UpdateBulletState;
    }
    private void InitWeaponSet()
    {
        _pistol.gameObject.SetActive(false);
        _shotgun.gameObject.SetActive(false);
        _sniperRifle.gameObject.SetActive(false);
        _rifle.gameObject.SetActive(false);
        _grenadeThrower.gameObject.SetActive(false);
        currentWeapon = _pistol;
        currentWeapon.gameObject.SetActive(true);
    }

    public void OnPause()
    {
        CinemachineBrain cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        cameraBrain.m_IgnoreTimeScale = false;
        cameraBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate;
        currentWeapon.animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void OnResume()
    {
        CinemachineBrain cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        cameraBrain.m_IgnoreTimeScale = true;
        cameraBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        currentWeapon.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private IEnumerator WaitForEvadeTutorial()
    {
        WaitUntil waitUntil = new WaitUntil(() => GameManager.Instance.guideController.GuideEnded());

        yield return waitUntil;

        this.stateMachine.ChangeState(this.stateMachine.evadeState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trajectory"))
        {
            if (!PlayerPrefs.HasKey("SlowMotionGuide"))
            {
                GameManager.Instance.guideController.StartGuide(Resources.Load<GuideData>("Guide/SlowMotionGuide"));

                StartCoroutine(WaitForEvadeTutorial());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trajectory"))
        {
            isEvadable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trajectory"))
        {
            isEvadable = false;
        }
    }
    public void ChangeCameraView(bool isThird)
    {
        thirdPerson.SetActive(isThird);
        firstPerson.SetActive(!isThird);

        CameraManager.Instance.ChangeBlendTime(0.1f);
        CameraManager.Instance.ChangeBlendStyle(CinemachineBlendDefinition.Style.EaseInOut);

        if (isThird)
        {

            CameraManager.Instance.ChangeCameraView(stateMachine.player.thirdCamera);
            //stateMachine.player.thirdCamera.MoveToTopOfPrioritySubqueue();
        }
        else
        {
            CameraManager.Instance.ChangeCameraView(stateMachine.player.firstCamera);
            //stateMachine.player.firstCamera.MoveToTopOfPrioritySubqueue();
        }
    }
}
