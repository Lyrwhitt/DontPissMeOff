using System.Collections;
using UnityEngine;

public class BossRobotLeftArm : MonoBehaviour
{
    [SerializeField] private GameObject _bossRobotDestroyedLeftArmObj;
    [SerializeField] private ParticleSystem[] _smallExplisions;
    [SerializeField] private ParticleSystem[] _bigExplisions;

    private void ActiveLeftArm()
    {
        _bossRobotDestroyedLeftArmObj.SetActive(true);
    }

    private void LeftArmSmallExplosion(int index)
    {
        if (index < 0 || index >= _smallExplisions.Length)
            return;

        _smallExplisions[index].Play();
    }
    private void LeftArmBigExplosion(int index)
    {
        if (index < 0 || index >= _bigExplisions.Length)
            return;

        _bigExplisions[index].Play();
    }

    private void DropLeftArm()
    {
        _bossRobotDestroyedLeftArmObj.transform.SetParent(null);
        _bossRobotDestroyedLeftArmObj.GetComponent<Rigidbody>().velocity = -2f * _bossRobotDestroyedLeftArmObj.transform.right;
        StartCoroutine(DestroyLeftArmCO());
    }

    IEnumerator DestroyLeftArmCO()
    {
        yield return new WaitForSecondsRealtime(5f);
        _bossRobotDestroyedLeftArmObj.SetActive(false);
    }

    private void PlayBeamSound()
    {
        SoundManager.Instance.Play("LazerBeam");
    }

}
