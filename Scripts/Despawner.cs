using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public float lifeTime = 2f;

    private void OnEnable()
    {
        Invoke("Deactivate", lifeTime);
    }

    private void Deactivate()
    {
        ObjectPool.Instance.ReturnToPool(tag, gameObject);
        gameObject.SetActive(false);
    }
}
