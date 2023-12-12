using UnityEngine;

public class BossRobotStompController : MonoBehaviour
{
    [SerializeField] private GameObject _shockwaveObj;
    [SerializeField] private int _damage = 30;

    private ParticleSystem _shockwaveParticle;

    private void Start()
    {
        _shockwaveParticle = _shockwaveObj.GetComponent<ParticleSystem>();
    }

    public void Stomp()
    {
        _shockwaveObj.transform.position = transform.position;
        _shockwaveParticle.Play();
        AddForceToPlayer();
    }

    private void AddForceToPlayer()
    {
        Debug.Log("AddForce");
        Ray ray = new Ray(transform.parent.position + new Vector3(0f, 1f, 0f), transform.parent.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast");

            if ((hit.collider.CompareTag("Player")))
            {
                if(hit.transform.TryGetComponent(out ForceReceiver forceReceiver))
                {
                    forceReceiver.AddForce(transform.parent.forward * 50f);
                }

                if (hit.transform.TryGetComponent(out Health health))
                {
                    health.TakeDamage(_damage);
                }
            }
        }
    }
}
