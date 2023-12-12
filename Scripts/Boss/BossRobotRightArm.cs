using System.Collections;
using UnityEngine;

public class BossRobotRightArm : MonoBehaviour
{
    [SerializeField] private GameObject _bossRobotDestroyedRightArmObj;
    [SerializeField] private ParticleSystem[] _smallExplisions;
    [SerializeField] private ParticleSystem[] _bigExplisions;

    private void ActiveRightArm()
    {
        _bossRobotDestroyedRightArmObj.SetActive(true);
    }

    private void RightArmSmallExplosion(int index)
    {
        if (index < 0 || index >= _smallExplisions.Length)
            return;

        _smallExplisions[index].Play();
    }

    private void RightArmBigExplosion(int index)
    {
        if (index < 0 || index >= _bigExplisions.Length)
            return;

        _bigExplisions[index].Play();
    }

    private void DropRightArm()
    {
        _bossRobotDestroyedRightArmObj.transform.SetParent(null);
        _bossRobotDestroyedRightArmObj.GetComponent<Rigidbody>().velocity = 2f * _bossRobotDestroyedRightArmObj.transform.right;
        StartCoroutine(DestroyRightArmCO());
    }

    IEnumerator DestroyRightArmCO()
    {
        yield return new WaitForSecondsRealtime(5f);
        _bossRobotDestroyedRightArmObj.SetActive(false);
    }
}
