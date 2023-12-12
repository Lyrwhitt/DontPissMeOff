using UnityEngine;

public class BossRobotShoulderMissile : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _childObj;
    [SerializeField] private GameObject _parentObj;
    [SerializeField] private int _damage = 20;

    private GameObject _explosionObj;
    private ParticleSystem _explsionParticle;

    private void Start()
    {
        _explosionObj = Instantiate(_explosionPrefab, _parentObj.transform);
        _explsionParticle = _explosionObj.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
            health.TakeDamage(_damage);

        Explose();
    }

    public void Explose()
    {
        if (_explsionParticle != null)
        {
            _explosionObj.transform.position = this.gameObject.transform.position;
            _explsionParticle.Play();
        }
        SoundManager.Instance.Play("Explosion");
        _childObj.SetActive(false);
    }

    public void RemoveMissile()
    {
        _childObj.SetActive(true);
    }
}
