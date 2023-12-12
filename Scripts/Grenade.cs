using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public bool isThorow = false;
    public float explosionDelay = 3f; // 수류탄이 터지기까지의 딜레이
    public ParticleSystem explosionEffect; // 폭발 효과를 위한 파티클 시스템
    public float explosionRadius = 5f; // 폭발 반경
    public float explosionForce = 700f; // 폭발 힘
    public int explosionDamage = 300;

    private void OnEnable()
    {
        isThorow = false;
    }

    void Update()
    {
        if (isThorow)
            StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);

        // 폭발 효과를 재생
        ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.Play();

        // 대미지
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Health health = nearbyObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(explosionDamage); // 대미지 값을 원하는 값으로 설정해야 합니다.
                GameManager.Instance.damageUI.ShowDamageText(explosionDamage, health.transform.position);
            }

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);

        yield return new WaitForSeconds(explosion.main.duration);
        Destroy(explosion.gameObject);
    }
}
