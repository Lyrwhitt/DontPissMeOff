using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [SerializeField] ParticleSystem _missileFlame1;
    [SerializeField] ParticleSystem _missileFlame2;

    public void FlameMissile1()
    {
        _missileFlame1.Play();
    }

    public void FlameMissile2()
    {
        _missileFlame2.Play();
    }
}
