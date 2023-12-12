using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class EventBullet : MonoBehaviour
{
    private float _speed;
    private Vector3 _prevPos;
    private Vector3 _direction;

    private CinemachineImpulseSource _impulseSource;

    [HideInInspector] public bool isDamagable = false;
    [HideInInspector] public float damage;
    public float lifeTime = 2f;

    private void Awake()
    {
        _impulseSource = this.GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        Invoke("Deactivate", lifeTime);

        _prevPos = transform.position;
    }

    private void Update()
    {
        _prevPos = transform.position;

        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        RaycastHit[] hits = Physics.RaycastAll(new Ray(_prevPos, (transform.position - _prevPos).normalized),
            (transform.position - _prevPos).magnitude);

        if (hits.Length > 0)
        {
            Collider collision = hits[0].collider;
            Vector3 normal = hits[0].normal;
            Vector3 point = hits[0].point;

            if (isDamagable)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (collision.transform.TryGetComponent(out Health health))
                    {
                        if (health.isinvincible)
                            return;
                        
                        health.TakeDamage((int)damage);
                        _impulseSource.GenerateImpulse(Camera.main.transform.forward);
                    }
                }
            }

            gameObject.SetActive(false);
        }
    }

    public void SetBullet(float speed, Vector3 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    private void Deactivate()
    {
        ObjectPool.Instance.ReturnToPool("EventBullet", this.gameObject);
        this.gameObject.SetActive(false);
    }
}
