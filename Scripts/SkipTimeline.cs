using UnityEngine;
using UnityEngine.Playables;

public class SkipTimeline : MonoBehaviour
{
    [SerializeField] private float _time;

    public void Init()
    {
        TimelineManager.Instance.OnSkipTime();
        TimelineManager.Instance.playableDirector = GetComponent<PlayableDirector>();
        TimelineManager.Instance.SkipTime = SkipTime;
    }

    public void SkipTime()
    {
        TimelineManager.Instance.playableDirector.time = _time;
    }

    public void OffSkipTime()
    {
        TimelineManager.Instance.OffSkipTime();
    }
}
