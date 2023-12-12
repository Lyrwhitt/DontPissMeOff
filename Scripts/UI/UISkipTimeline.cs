using UnityEngine;
using UnityEngine.UI;

public class UISkipTimeline : MonoBehaviour
{
    [SerializeField] private GameObject _backgroundObj;
    [SerializeField] private Image _image;
    private float _curFillAmount;
    private bool _isFull;

    private void Start()
    {
        Init();
        _backgroundObj.SetActive(false);
    }

    public void Init()
    {
        _backgroundObj.SetActive(true);
        _isFull = false;
        _curFillAmount = 0f;
        _image.fillAmount = _curFillAmount;
    }

    public void ChangeFillAmount(float amount)
    {
        if (_isFull)
            return;

        _curFillAmount += amount;

        _curFillAmount = _curFillAmount < 0f ? 0f : _curFillAmount;

        if (_curFillAmount >= 1f)
        {
            _curFillAmount = 1f;
            _isFull = true;
            DeActivate();
        }

        _image.fillAmount = _curFillAmount;
    }

    public bool IsFull()
    {
        return _isFull;
    }

    public void DeActivate()
    {
        _backgroundObj.SetActive(false);
    }
}
