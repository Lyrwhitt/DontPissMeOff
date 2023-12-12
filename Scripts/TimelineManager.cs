using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;

    public PlayableDirector playableDirector;
    public Action SkipTime;

    private GameObject _skipTimelineObj;
    private UISkipTimeline _uISkipTimeline;
    private float _changeAmount = 0.01f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        GameObject obj = Resources.Load<GameObject>("UI/SkipTimelineUI");
        _skipTimelineObj = Instantiate(obj, transform);
        _uISkipTimeline = _skipTimelineObj.GetComponent<UISkipTimeline>();
    }

    private void Update()
    {
        if (_uISkipTimeline != null && playableDirector != null)
        {
            if (_uISkipTimeline.IsFull())
            {
                SkipTime?.Invoke();
                OffSkipTime();
            }


            if (Input.GetKey(KeyCode.Q))
            {
                _uISkipTimeline.ChangeFillAmount(_changeAmount);
            }
            else
            {
                _uISkipTimeline.ChangeFillAmount(-_changeAmount);
            }
        }
    }

    public void OnSkipTime()
    {
        _uISkipTimeline.Init();
    }

    public void OffSkipTime()
    {
        playableDirector = null;
        _uISkipTimeline.DeActivate();
    }
}
