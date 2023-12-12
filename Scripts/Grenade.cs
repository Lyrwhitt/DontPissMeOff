using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public bool isThorow = false;
    public float explosionDelay = 3f; // ����ź�� ����������� ������
    public ParticleSystem explosionEffect; // ���� ȿ���� ���� ��ƼŬ �ý���
    public float explosionRadius = 5f; // ���� �ݰ�
    public float explosionForce = 700f; // ���� ��
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

        // ���� ȿ���� ���
        ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.Play();

        // �����
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Health health = nearbyObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(explosionDamage); // ����� ���� ���ϴ� ������ �����ؾ� �մϴ�.
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
