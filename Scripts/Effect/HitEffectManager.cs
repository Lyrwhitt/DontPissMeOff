using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance { get; private set; }

    [SerializeField] private GameObject bulletHitEffectPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<GameObject> bulletHitEffectPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitializePool();
    }
    void Start()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletHitEffect = Instantiate(bulletHitEffectPrefab, Vector3.zero, Quaternion.identity);
            bulletHitEffect.gameObject.transform.SetParent(gameObject.transform);
            bulletHitEffect.SetActive(false);
            bulletHitEffectPool.Enqueue(bulletHitEffect);
        }
    }

    public GameObject GetBulletHitEffect(Vector3 position)
    {
        if (bulletHitEffectPool.Count == 0)
        {
            // Pool is empty, create a new instance
            GameObject bulletHitEffect = Instantiate(bulletHitEffectPrefab, position, Quaternion.identity);
            return bulletHitEffect;
        }
        else
        {
            GameObject bulletHitEffect = bulletHitEffectPool.Dequeue();
            bulletHitEffect.transform.position = position;
            bulletHitEffect.SetActive(true);
            return bulletHitEffect;
        }
    }

    public void ReturnBulletHitEffect(GameObject bulletHitEffect)
    {
        bulletHitEffect.SetActive(false);
        bulletHitEffectPool.Enqueue(bulletHitEffect);
    }
}
