using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossRobotOpening : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private TimelineAsset _timelineClip;
    [SerializeField] private GameObject _map;
    public GameObject minimap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.Play("BossBGM", SoundManager.Sound.Bgm);

            minimap.SetActive(false);
            _map.SetActive(false);
            _playableDirector.Play(_timelineClip);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
