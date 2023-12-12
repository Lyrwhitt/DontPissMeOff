using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    private bool _isCounting = false;
    private float _countDown;

    public TextMeshProUGUI countDownText;

    private void Update()
    {
        if (!_isCounting)
            return;

        if(_countDown > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _countDown -= 1f;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RetryButtonDown();
            }

            _countDown -= Time.deltaTime;
            countDownText.text = _countDown.ToString("N0");
        }
        else
        {
            _countDown = 0f;
            _isCounting = false;

            EndCount();
        }
    }

    public void StartCounting()
    {
        _isCounting = true;
        _countDown = 10f;
    }

    private void RetryButtonDown()
    {
        _isCounting = false;
        _countDown = 10f;

        this.gameObject.SetActive(false);

        //GameManager.Instance.RetryGame();

        PlayerPrefs.SetString("NewGame", "False");
        SceneManager.LoadSceneAsync("GameScene");
    }

    private void EndCount()
    {
        // 인트로씬으로 이동 할것
    }
}
