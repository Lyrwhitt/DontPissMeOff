using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    private CinemachineBrain _brain;
    [field: HideInInspector] public CinemachineVirtualCamera currentCam;

    [HideInInspector]
    public Volume cameraFilter;

    private static CameraManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (!PlayerPrefs.HasKey("Mouse"))
        {
            PlayerPrefs.SetFloat("Mouse", 1f);
        }
    }

    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    void Start()
    {
        _brain = Camera.main.GetComponent<CinemachineBrain>();
        cameraFilter = this.GetComponentInChildren<Volume>();
    }

    public void ChangeCameraSpeed(float speed)
    {
        if(currentCam == null)
            return;

        CinemachinePOV pov = currentCam.GetCinemachineComponent<CinemachinePOV>();

        if (pov != null)
        {
            pov.m_HorizontalAxis.m_MaxSpeed = speed;
            pov.m_VerticalAxis.m_MaxSpeed = speed;
        }
    }

    public void ChangeCameraView(CinemachineVirtualCamera camera)
    {
        currentCam = camera;
        ChangeCameraSpeed(PlayerPrefs.GetFloat("Mouse"));

        camera.MoveToTopOfPrioritySubqueue();
    }

    public void ChangeCameraView(CinemachineVirtualCamera camera, float blendTimeOnce)
    {
        currentCam = camera;
        ChangeCameraSpeed(PlayerPrefs.GetFloat("Mouse"));

        float originalBelnd = _brain.m_DefaultBlend.m_Time;

        ChangeBlendTime(blendTimeOnce);

        camera.MoveToTopOfPrioritySubqueue();

        ChangeBlendTime(originalBelnd);
    }

    public void ChangeBlendTime(float time)
    {
        _brain.m_DefaultBlend.m_Time = time;
    }

    public void ChangeBlendStyle(CinemachineBlendDefinition.Style style)
    {
        _brain.m_DefaultBlend.m_Style = style;
    }

    public CinemachineFollowZoom TryGetFollowZoomCamera()
    {
        if (_brain.ActiveVirtualCamera.VirtualCameraGameObject.TryGetComponent(out CinemachineFollowZoom followZoom))
        {
            return followZoom;
        }
        else
            return null;
    }
}
