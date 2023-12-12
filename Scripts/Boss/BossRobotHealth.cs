using System;
using UnityEngine;

public class BossRobotHealth : MonoBehaviour
{
    [SerializeField] private Health[] _healths;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    public event Action OnDie;

    private UIHPBar _uIHPBar;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (_health >= 0)
            UpdateHealth();
    }

    private void UpdateHealth()
    {
        int totalDamage = 0;
        for (int i = 0; i < _healths.Length; i++)
        {
            totalDamage += (_maxHealth - _healths[i].health);
        }
        _health = _maxHealth - totalDamage >= 0 ? _maxHealth - totalDamage : 0;

        if (_health == 0)
            OnDie?.Invoke();

        _uIHPBar?.SetCurHp(_health);
    }

    public int GetCurHealth()
    {
        return _health;
    }

    public void SetUIHPBar(UIHPBar uIHPBar)
    {
        _uIHPBar = uIHPBar;
        _uIHPBar?.Initialize(_maxHealth);
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
        for (int i = 0; i < _healths.Length; i++)
            _healths[i].SetMaxHealth(maxHealth);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _healths.Length; i++)
            _healths[i].enabled = false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _healths.Length; i++)
            _healths[i].enabled = true;
    }
}
