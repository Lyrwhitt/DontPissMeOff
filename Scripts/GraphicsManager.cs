using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager Instance;

    [Header("Graphics")]
    [SerializeField] private List<RenderPipelineAsset> RenderPipelineAssets;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SetGraphicsData();
    }

    private void SetGraphicsData()
    {
        if (!PlayerPrefs.HasKey("Graphic"))
        {
            PlayerPrefs.SetInt("Graphic", 2);
        }
        else
        {
            SetPipeline(PlayerPrefs.GetInt("Graphic"));
        }
    }

    public void SetPipeline(int value)
    {
        PlayerPrefs.SetInt("Graphic", value);

        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = RenderPipelineAssets[value];
    }
}
