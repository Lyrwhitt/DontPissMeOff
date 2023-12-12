using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventScenario : MonoBehaviour
{
    [SerializeField]
    private EventSO eventData;

    [SerializeField]
    private UIEventMode eventModeUI;

    public EventTrigger eventTrigger;

    [SerializeField]
    private int _enemyCount = 0;

    public bool isClear = false;
    public bool isStart = false;

    private List<EventEnemy> enemies = new List<EventEnemy>();
    private Transform enemySet;

    public Player player;

    private float _timer;

    public CinemachineVirtualCamera hideCamera;
    public CinemachineVirtualCamera shotCamera;

    public UnityEvent onEndEvent;

    private void Awake()
    {
        SetEvent();
    }


    private void Update()
    {
        if (_timer > 0f && isStart && !isClear)
        {
            _timer -= Time.deltaTime;

            if(_timer <= 0f)
            {
                _timer = 0f;

                OnFailEvent();
            }

            eventModeUI.ChangeTimerValue(_timer);
        }
    }

    public void OnRetry()
    {
        eventTrigger.gameObject.SetActive(true);

        if(enemySet != null)
            EnemyClear();

        SetEvent();

        eventTrigger.CutSceneStart();
    }

    private void SetEvent()
    {
        _enemyCount = 0;
        isStart = false;
        _timer = eventData.timer;

        GameObject newEnemySet = Instantiate(eventData.enemySet, this.transform, false);
        enemySet = newEnemySet.transform;

        for (int i = 0; i < enemySet.childCount; i++)
        {
            _enemyCount += 1;
            enemies.Add(enemySet.GetChild(i).GetComponent<EventEnemy>());
            enemies[i].EnemyHealth.OnDie += OnEnemyDied;
        }
    }

    public void StartEventMode()
    {
        if (!PlayerPrefs.HasKey("EventModeGuide"))
            GameManager.Instance.guideController.StartGuide(Resources.Load<GuideData>("Guide/EventModeGuide"));


        GameManager.Instance.currentEvent = this;

        // 리트라이 로직 변경에 따라 잠시 막아둡니다.
        //GameManager.Instance.SaveLastData();

        if (player.currentEvent != null)
        {
            player.currentEvent.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }

        isStart = true;

        player.SetCurrentEventScenario(this);
        player.stateMachine.ChangeState(player.stateMachine.eventState);

        eventModeUI.gameObject.SetActive(true);
    }

    public void OnEnemyDied()
    {
        _enemyCount--;

        if (_enemyCount == 0)
        {
            OnClearEvent();
        }
    }

    public void OnClearEvent()
    {
        GameManager.Instance.currentEvent = null;

        EnemyClear();

        isClear = true;

        player.stateMachine.ChangeState(player.stateMachine.idleState);

        eventModeUI.gameObject.SetActive(false);
        eventModeUI.InitEventUI();

        onEndEvent?.Invoke();
    }

    public void EnemyClear()
    {
        Destroy(enemySet.gameObject);
        enemies.Clear();
    }

    public void OnFailEvent()
    {
        player.status.TakeDamage(player.status.health);
    }
}
