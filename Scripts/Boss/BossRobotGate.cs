using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BossRobotGate : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _gateExplosion1;
    [SerializeField] private ParticleSystem[] _gateExplosion2;
    [SerializeField] private GameObject _timelineTriggerObj;
    [SerializeField] private int _eventCount;

    [field: Header("TimeLine")]
    [SerializeField] private PlayableDirector _script3Director;
    [SerializeField] private PlayableDirector _fireMissileDirector;

    public void EventCleared()
    {
        _eventCount -= 1;
        if (_eventCount <= 0)
        {
            _timelineTriggerObj.SetActive(true);
            _script3Director.Play();
        }
    }

    public void PlayFireMissileToGateTimeline()
    {
        _timelineTriggerObj.SetActive(false);
        _fireMissileDirector.Play();
    }

    public void ExplodeGate1()
    {
        StartCoroutine(ExplodeGate1CO());
    }

    IEnumerator ExplodeGate1CO()
    {
        for(int i = 0; i  < _gateExplosion1.Length; i++)
        {
            _gateExplosion1[i].Play();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void ExplodeGate2()
    {
        StartCoroutine(ExplodeGate2CO());
    }

    IEnumerator ExplodeGate2CO()
    {
        for (int i = 0; i < _gateExplosion2.Length; i++)
        {
            _gateExplosion2[i].Play();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
