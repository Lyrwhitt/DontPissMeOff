using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    public UnityEvent response;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            response?.Invoke();
    }
}
