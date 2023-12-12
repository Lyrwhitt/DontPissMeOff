using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO Data { get; set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    [field: SerializeField] public Weapon Weapon { get; private set; }
    [field: SerializeField] public SphereCollider Head { get; private set; }
    public Health EnemyHealth { get; private set; }
    public Animator Animator { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public CharacterController Controller { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }

    public StatsChangeType statsChangeType;
    [field: SerializeField] public TargetPositionData PosData { get; private set; }



    private EnemyStateMachine stateMachine;
    public Transform originalPosition;

    private WaitForSeconds waitForSeconds;


    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        EnemyHealth = GetComponent<Health>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        originalPosition = transform;
        stateMachine = new EnemyStateMachine(this);
        if(statsChangeType == StatsChangeType.Sniper || statsChangeType == StatsChangeType.Sniper2)
        {
            AnimationData.SniperInit();
        }
        if (statsChangeType == StatsChangeType.Elite)
        {
            AnimationData.EliteInit();
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
        EnemyHealth.OnDie += OnDie;
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
    void OnDie()
    {
        stateMachine.MovementSpeedModifier = 0f;
        NavMeshAgent.enabled = false;
        Controller.enabled = false;
        Head.enabled = false;
        Animator.SetTrigger("Die");
        enabled = false;
        StartCoroutine(DisableCoroutine(5f));
        //Destroy(gameObject, 5f);
    }

    private IEnumerator DisableCoroutine(float time)
    {
        waitForSeconds = new WaitForSeconds(time);

        yield return waitForSeconds;

        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (stateMachine == null || stateMachine.Enemy == null)
        {
            return;
        }

        // 원 표시
        Vector3 enemyPosition = stateMachine.Enemy.transform.position + new Vector3(0, 1f, 0) + stateMachine.Enemy.transform.forward;
        float attackRange = stateMachine.Enemy.Data.AttackRange;

        Gizmos.color = Color.red;

        // 레이 표시
        Vector3 rayStart = enemyPosition;
        Vector3 rayEnd = enemyPosition + stateMachine.Enemy.transform.forward * attackRange;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayStart, rayEnd);
    }
}
