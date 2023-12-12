using UnityEngine;

public class BossRobotLeftArmBeam : MonoBehaviour
{
    [SerializeField] private GameObject _flamePrefab;
    [SerializeField] private GameObject _flameParentObj;
    [SerializeField] private int _damage = 20;

    private GameObject _flameObj;
    private ParticleSystem _flameParticle;

    private void Start()
    {
        _flameObj = Instantiate(_flamePrefab, _flameParentObj.transform);
        _flameParticle = _flameObj.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Beam();
    }

    private void Beam()
    {
        Ray ray = new Ray(transform.parent.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage);
            }

            Vector3 hitPosition = hit.point;
            _flameObj.transform.position = hitPosition;

            if (!_flameParticle.isPlaying)
                _flameParticle.Play();
        }
    }

    //왜 안되는지 나중에 알아볼 것
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (_flameParticle != null)
    //    {
    //        ContactPoint contact = collision.contacts[0];
    //        _flameObj.transform.position = contact.point;
    //        Debug.Log($"{collision.transform.name} : First Point : {contact.point}");

    //        if (!_flameParticle.isPlaying)
    //            _flameParticle.Play();
    //    }
    //}

}

