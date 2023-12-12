using UnityEngine;

public class Bullet: MonoBehaviour
{
    private float _speed;
    private Vector3 _prevPos;
    private Vector3 _direction;

    [HideInInspector] public float damage;
    public float lifeTime = 2f;

    private GameObject _hitEffect;
    public TrailRenderer trailRenderer;
    public GameObject owner;

    private void OnEnable()
    {
        Invoke("Deactivate", lifeTime);
        _prevPos = transform.position;
    }

    public void SetBullet(float speed, Vector3 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    private void Update()
    {
        _prevPos = transform.position;

        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        // "Block", "Ignore", "Trajectory" 레이어 제외
        int layerMask = ~((1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));

        RaycastHit[] hits = Physics.RaycastAll(new Ray(_prevPos, (transform.position - _prevPos).normalized),
            (transform.position - _prevPos).magnitude, layerMask);

        if (hits.Length > 0)
        {
            Collider collision = hits[0].collider;
            Vector3 normal = hits[0].normal;
            Vector3 point = hits[0].point;

            if (collision.gameObject.tag != "Bullet" && collision.gameObject != owner)
            {
                Quaternion hitRotation = Quaternion.FromToRotation(Vector3.forward, normal) * Quaternion.AngleAxis(90f, Vector3.right);
                if (collision.transform.TryGetComponent(out Health health))
                {
                    if (collision.gameObject.tag == "Enemy")
                    {
                        if (owner.CompareTag("Player"))
                        {
                            ObjectPool.Instance.SpawnFromPool("Blood", point, hitRotation);
                            if (!health.isinvincible && health.health != 0)
                                GameManager.Instance.damageUI.ShowDamageText((int)damage, point);
                        }
                    }
                    if (collision.gameObject.tag == "Player")
                    {
                        GameManager.Instance.playerHit.ShowHitEffect(point);
                    }
                    health.TakeDamage((int)damage);
                }
                else
                {
                    if (collision.gameObject.tag == "Head")
                    {
                        if (owner.CompareTag("Player"))
                        {
                            ObjectPool.Instance.SpawnFromPool("Blood", point, hitRotation);
                            Health hp = collision.transform.parent.gameObject.GetComponent<Health>();  
                            if (!hp.isinvincible && hp.health != 0)
                                GameManager.Instance.damageUI.ShowDamageText((int)damage * 2, point,DamageType.Head);
                            hp.TakeDamage((int)damage * 2);
                        }
                    }
                    _hitEffect = ObjectPool.Instance.SpawnFromPool("HitEffect", point, hitRotation);
                    Invoke("ReturnHitEffectToPool", 1f);
                }

                gameObject.SetActive(false);
            }
        }
    }
    private void Deactivate()
    {
        ObjectPool.Instance.ReturnToPool(tag, gameObject);
        gameObject.SetActive(false);
    }

    private void ReturnHitEffectToPool()
    {
        ObjectPool.Instance.ReturnToPool(_hitEffect);
    }
}