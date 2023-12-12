using UnityEngine;
using UnityEngine.Playables;

public class BossRobotBody : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _smallExplisions;
    [SerializeField] private ParticleSystem[] _bigExplisions;
    [SerializeField] private PlayableDirector _playableDirector;

    private void BodySmallExplosion(int index)
    {
        if(index < 0 || index >= _smallExplisions.Length)
            return;

        _smallExplisions[index].Play();
    }

    private void BodyBigExplosion(int index)
    {
        if (index < 0 || index >= _bigExplisions.Length)
            return;

        _bigExplisions[index].Play();
    }

    private void PlayRepairTimeLine()
    {
        _playableDirector.Play();
    }

    private void PlayWalkingSound()
    {
        SoundManager.Instance.Play("FootSteps");
    }
}
