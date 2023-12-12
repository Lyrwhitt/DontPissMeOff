using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : UIPopup
{
    [Header("Graphics")]
    public Toggle lowGraphicToggle;
    public Toggle mediumGraphicToggle;
    public Toggle highGraphicToggle;

    [Header("Sound")]
    public Slider bgmSlider;
    public Slider effectSlider;

    [Header("Mouse")]
    public Slider mouseSensitivitySlider;
    public Text mouseSeneitivityValueText;

    public Button cancelBtn;


    private void Awake()
    {
        cancelBtn.onClick.AddListener(OnCancelBtnClicked);

        bgmSlider.onValueChanged.AddListener(OnBgmValueChanged);
        effectSlider.onValueChanged.AddListener(OnEffectValueChanged);

        lowGraphicToggle.onValueChanged.AddListener(OnLowGraphicClicked);
        mediumGraphicToggle.onValueChanged.AddListener(OnMideumGraphicClicked);
        highGraphicToggle.onValueChanged.AddListener(OnHighGraphicClicked);

        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityValueChanged);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("BGM"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        }
        if (PlayerPrefs.HasKey("Effect"))
        {
            effectSlider.value = PlayerPrefs.GetFloat("Effect");
        }
        if (PlayerPrefs.HasKey("Graphic"))
        {
            int value = PlayerPrefs.GetInt("Graphic");

            //SetPipeline(value);

            switch(value)
            {
                case 0:
                    lowGraphicToggle.isOn = true;
                    break;
                case 1:
                    mediumGraphicToggle.isOn = true;
                    break;
                case 2:
                    highGraphicToggle.isOn = true;
                    break;
            }
        }
        else
        {
            highGraphicToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("Mouse"))
        {
            float sensitivity = PlayerPrefs.GetFloat("Mouse");
            mouseSensitivitySlider.value = sensitivity;
            mouseSeneitivityValueText.text = sensitivity.ToString();
        }
    }

    private void OnMouseSensitivityValueChanged(float value)
    {
        value = Mathf.Round(value * 10.0f) * 0.1f;
        mouseSensitivitySlider.value = value;
        mouseSeneitivityValueText.text = value.ToString();
    }

    private void OnCancelBtnClicked()
    {
        SavePrefs();

        if(SceneManager.GetActiveScene().name == "GameScene")
            CameraManager.Instance.ChangeCameraSpeed(mouseSensitivitySlider.value);

        UIManager.Instance.ClosePopup();

        if(UIManager.Instance.IsOpenedPopup("UIPause"))
            UIManager.Instance.GetPopup().gameObject.SetActive(true);
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetFloat("Mouse", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        PlayerPrefs.SetFloat("Effect", effectSlider.value);
    }

    private void OnBgmValueChanged(float value)
    {
        SoundManager.Instance.GetAudioSource(SoundManager.Sound.Bgm).volume = value;
    }

    private void OnEffectValueChanged(float value)
    {
        SoundManager.Instance.GetAudioSource(SoundManager.Sound.Effect).volume = value;
    }

    private void OnLowGraphicClicked(bool value)
    {
        if (value)
        {
            GraphicsManager.Instance.SetPipeline(0);
        }
    }
    private void OnMideumGraphicClicked(bool value)
    {
        if (value)
        {
            GraphicsManager.Instance.SetPipeline(1);
        }
    }
    private void OnHighGraphicClicked(bool value)
    {
        if (value)
        {
            GraphicsManager.Instance.SetPipeline(2);
        }
    }
}
