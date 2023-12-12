using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BossRobot : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _robot;

    [field: Header("Health")]
    [SerializeField] private BossRobotHealth _leftArmHealth;
    [SerializeField] private BossRobotHealth _rightArmHealth;
    [SerializeField] private BossRobotHealth _bodyHealth;
    [SerializeField] private GameObject _bodyObj;
    [SerializeField] private GameObject _leftArmObj;
    [SerializeField] private GameObject _leftHandObj;
    [SerializeField] private GameObject _rightArmObj;
    [SerializeField] private GameObject _rightHandObj;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private int _leftArmMaxHealth;
    [SerializeField] private int _rightArmMaxHealth;
    [SerializeField] private int _bodyMaxHealth;

    [field: Header("Animations")]
    [field: SerializeField] public BossAnimationData AnimationData { get; private set; }
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private GameObject _minAttackRangeObj;
    [SerializeField] private GameObject _maxAttackRangeObj;
    [SerializeField] private GameObject _attackHeightObj;
    [SerializeField] private float _attackDelayTime = 5f;
    [SerializeField] private float _attackMissileDelayTime = 10f;
    [SerializeField] private float _attackStompDelayTime = 3f;

    [field: Header("TimeLine")]
    [SerializeField] private PlayableDirector _endingDirector;

    private GameObject _leftHPBar;
    private GameObject _rightHPBar;
    private GameObject _bodyHPBar;
    private Collider _bodyCollider;
    private bool _isLeftArmHealthZero = false;
    private bool _isRightArmHealthZero = false;
    private bool _isTimeToRepair = false;
    private bool _isFinalPhase = false;
    private bool _isDead = false;

    private Coroutine _bodyCoroutine;

    private BossRobotStateMachine stateMachine;


    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        AnimationData.Initialize();
        stateMachine = new BossRobotStateMachine(this);
    }

    private void Start()
    {
        _bodyCollider = _bodyObj.GetComponent<Collider>();
        _bodyCollider.enabled = false;

        stateMachine.ChangeState(stateMachine.IdleState);

        _leftHPBar = Instantiate(_hpBar, _canvas.transform);
        _rightHPBar = Instantiate(_hpBar, _canvas.transform);
        _bodyHPBar = Instantiate(_hpBar, _canvas.transform);

        _leftHPBar.transform.localPosition = new Vector3(-4f, 0f, 0f);
        _rightHPBar.transform.localPosition = new Vector3(4f, 0f, 0f);
        _bodyHPBar.transform.localPosition = new Vector3(0f, 0f, 0f);
        _leftHPBar.SetActive(false);
        _rightHPBar.SetActive(false);
        _bodyHPBar.SetActive(false);

        _leftArmHealth.SetMaxHealth(_leftArmMaxHealth);
        _rightArmHealth.SetMaxHealth(_rightArmMaxHealth);
        _bodyHealth.SetMaxHealth(_bodyMaxHealth);

        _leftHPBar.TryGetComponent(out UIHPBar leftHPBar);
        _leftArmHealth.SetUIHPBar(leftHPBar);
        _rightHPBar.TryGetComponent(out UIHPBar rightHPBar);
        _rightArmHealth.SetUIHPBar(rightHPBar);
        _bodyHPBar.TryGetComponent(out UIHPBar bodyHPBar);
        _bodyHealth.SetUIHPBar(bodyHPBar);

        _leftArmHealth.OnDie += DeactivateLeftArm;
        _rightArmHealth.OnDie += DeactivateRightArm;
        _bodyHealth.OnDie += Die;

        _leftArmHealth.enabled = false;
        _rightArmHealth.enabled = false;
        _bodyHealth.enabled = false;
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

    IEnumerator ActiveBodyColliderCO()
    {
        yield return null;

        while (true)
        {
            yield return null;

            if(_isLeftArmHealthZero && _isRightArmHealthZero)
            {
                SetActiveBody(true);
                StartCoroutine(ActiveRepairCO());
                break;
            }
        }
    }

    IEnumerator ActiveRepairCO()
    {
        yield return null;

        while (true)
        {
            yield return null;

            if (_bodyHealth.GetCurHealth() <= (_bodyMaxHealth / 2))
            {
                SetActiveBody(false);
                _isTimeToRepair = true;
                break;
            }
        }
    }

    private void SetActiveBody(bool value)
    {
        _bodyHealth.enabled = value;
        _bodyCollider.enabled = value;
        _bodyHPBar.SetActive(value);
    }

    public void DeactivateLeftArm()
    {
        _isLeftArmHealthZero = true;
        _leftHPBar.SetActive(false);
        _leftArmObj.SetActive(false);
        _leftHandObj.SetActive(false);
    }

    public void DeactivateRightArm()
    {
        _isRightArmHealthZero = true;
        _rightHPBar.SetActive(false);
        _rightArmObj.SetActive(false);
        _rightHandObj.SetActive(false);
    }

    public void Die()
    {
        _isDead = true;
        _bodyHPBar.SetActive(false);
        _endingDirector.Play();
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }

    public float GetMinAttackDistance()
    {
        return _minAttackRangeObj.transform.localPosition.z;
    }

    public float GetMaxAttackDistance()
    {
        return _maxAttackRangeObj.transform.localPosition.z;
    }

    public float GetAttactHeight()
    {
        return _attackHeightObj.transform.localPosition.y;
    }

    public float GetAttackDelayTime()
    {
        return _attackDelayTime;
    }

    public float GetAttackMissileDelayTime()
    {
        return _attackMissileDelayTime;
    }

    public float GetAttackStompDelayTime()
    {
        return _attackStompDelayTime;
    }

    public bool IsLeftArmHealthZero()
    {
        return _isLeftArmHealthZero;
    }

    public bool IsRightArmHealthZero()
    {
        return _isRightArmHealthZero;
    }

    public bool IsTimeToRepair()
    {
        return _isTimeToRepair;
    }

    public bool IsFinalPhase()
    {
        return _isFinalPhase;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void SetRepairTime(bool isTimeToRepair)
    {
        _isTimeToRepair = isTimeToRepair;
    }

    public void SetFinalPhase()
    {
        _isFinalPhase = true;
        _isTimeToRepair = false;
        SetActiveBody(true);
        _bodyHealth.SetMaxHealth(_bodyMaxHealth);
    }

    public void InitializeBossRobot()
    {
        if(_bodyCoroutine != null)
            StopCoroutine( _bodyCoroutine );
        _bodyCoroutine = StartCoroutine(ActiveBodyColliderCO());

        this.transform.localPosition = new Vector3(0f, 0f, 13f);
        _robot.transform.localPosition = Vector3.zero;
    }

    public void ActiveBothArmHealth()
    {
        _leftArmHealth.enabled = true;
        _rightArmHealth.enabled = true;
        _leftHPBar.SetActive(true);
        _rightHPBar.SetActive(true);
    }
}
