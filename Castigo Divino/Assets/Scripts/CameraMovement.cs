using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance; 

    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin;
    private float movementTime;
    private float totalMovementTime;
    private float initIntensity;

    private void Awake()
    {
        Instance = this;
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin = 
            CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    public void MoveCamera(float intensity, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        CinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        initIntensity = intensity;
        totalMovementTime = time;
        movementTime = time;
    }

    private void Update()
    {
        if (movementTime > 0)
        {
            movementTime -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                Mathf.Lerp(initIntensity, 0, 1 - (movementTime/totalMovementTime));
        }
    }
}
