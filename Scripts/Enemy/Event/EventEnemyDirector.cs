using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EventEnemyDirector : MonoBehaviour
{
    private EventScenario scenario;

    public bool movable = false;
    public bool standable = false;
    [HideInInspector] public List<Vector3> destinations = new List<Vector3>();
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float waitForAction = 1f;

    private EventEnemy _enemy;

    private Coroutine _coroutine;

    private void Awake()
    {
        _enemy = this.GetComponent<EventEnemy>();
        scenario = this.transform.parent.parent.gameObject.GetComponent<EventScenario>();
    }

    private void OnEnable()
    {
        if (movable)
        {
            MoveEnemy();
        }
        else if (standable)
        {
            StandEnemy();
        }
        else
        {
            WaitForStartEvent();
        }
    }
    public void StopAction()
    {
        StopCoroutine(_coroutine);
    }

    public void WaitForStartEvent()
    {
        _coroutine = StartCoroutine(WaitForStartEventCoroutine());
    }

    public void StandEnemy()
    {
        _enemy.Animator.SetTrigger(_enemy.AnimationData.StandParameterHash);

        _coroutine = StartCoroutine(WaitForAction());
    }

    public void MoveEnemy()
    {
        _enemy.Animator.SetBool(_enemy.AnimationData.RunParameterHash, true);

        _coroutine = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator WaitForStartEventCoroutine()
    {
        waitForAction = 1f;
        _enemy.isActioning = true;
        _enemy.controller.enabled = false;

        WaitUntil waitUntil = new WaitUntil(() => scenario.isStart);

        yield return waitUntil;

        _enemy.controller.enabled = true;

        StartCoroutine(WaitForAction());
    }

    private IEnumerator MoveCoroutine()
    {
        waitForAction = 1f;

        WaitUntil waitUntil = new WaitUntil(() => scenario.isStart);
        WaitForSeconds waitForSeconds = new WaitForSeconds(waitForAction);

        yield return waitUntil;

        _enemy.isActioning = true;

        for (int i = 0; i < destinations.Count; i++)
        {
            Vector3 destination = destinations[i];
            _enemy.transform.DOLookAt(destination, 0.5f);

            while (true)
            {
                if (Vector3.Distance(destination, _enemy.transform.position) < 0.01f)
                    break;

                _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position,
                    new Vector3(destination.x, _enemy.transform.position.y, destination.z), speed * Time.deltaTime);

                yield return null;
            }
        }

        _enemy.Animator.SetBool(_enemy.AnimationData.RunParameterHash, false);
        _enemy.Animator.SetTrigger(_enemy.AnimationData.BaseAttackParameterHash);


        Vector3 lookRotation = _enemy.GetLookRotation().eulerAngles;
        _enemy.transform.DOLocalRotate(lookRotation, 0.5f);

        yield return waitForSeconds;

        _enemy.isActioning = false;
    }

    private IEnumerator WaitForAction()
    {
        _enemy.Animator.SetTrigger(_enemy.AnimationData.BaseAttackParameterHash);

        yield return new WaitForNextFrameUnit();

        Vector3 lookRotation = _enemy.GetLookRotation().eulerAngles;
        _enemy.transform.DOLocalRotate(lookRotation, 0.5f);

        WaitForSeconds waitForSeconds = new WaitForSeconds(waitForAction);

        yield return waitForSeconds;

        _enemy.isActioning = false;
    }
}
