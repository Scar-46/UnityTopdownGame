using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Start()
    {
        StopShake();
    }

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float shakeForce, float shakeTime)
    {
        _cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cbmcp.m_AmplitudeGain = shakeForce;
        timer = shakeTime;
    }

    void StopShake()
    {
       _cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0;
        timer = 0;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                _cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cbmcp.m_AmplitudeGain = 0f;
            }
        }
    }

}
