using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPBar : MonoBehaviour
{
    [SerializeField] private Image _hpGauge;

    [SerializeField] private float _maxHp = 1f;
    [SerializeField] private float _curHp = 1f;

    private void Update()
    {
        _hpGauge.fillAmount = _curHp / _maxHp;
    }

    public void Initialize(float maxHp)
    {
        _maxHp = maxHp;
        _curHp = maxHp;
    }

    public void SetCurHp(float curHp)
    {
        _curHp = curHp;
    }
}
