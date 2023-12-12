using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private MeshRenderer trajectoryRenderer;

    public GameObject trajectory;

    [SerializeField] private Collider myCollider;

    private int damage;

    private List<Collider> alreadyColliderWith = new List<Collider>();

    public virtual void ShowOnTrajectory()
    {
        trajectoryRenderer.enabled = true;

        // SetLayer "Enemy"
        trajectory.layer = LayerMask.NameToLayer("Trajectory");
        //trajectory.SetActive(true);
    }

    public virtual void ShowOffTrajectory()
    {
        trajectoryRenderer.enabled = false;
        // SetLayer "Ignore"
        trajectory.layer = LayerMask.NameToLayer("Ignore");

        //trajectory.SetActive(false);
    }

    private void Start()
    {
        if (trajectory != null)
        {
            trajectoryRenderer = trajectory.GetComponent<MeshRenderer>();

            ShowOffTrajectory();

            myCollider.GetComponent<Health>().OnDie += ShowOffTrajectory;
        }
    }

    private void OnEnable()
    {
        alreadyColliderWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyColliderWith.Contains(other)) return;

        alreadyColliderWith.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            if (other.gameObject.tag == "Player")
                GameManager.Instance.playerHit.ShowHitEffect();
        }
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
