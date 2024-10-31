using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera idleCam;
    [SerializeField] private CinemachineVirtualCamera followCam;

    private void Awake()
    {
        SwitchCameraToIdle();
    }

    public void SwitchCameraToIdle()
    {
        idleCam.enabled = true;
        followCam.enabled = false;
    }

    public void SwitchCameraToFollow(Transform followPos)
    {
        followCam.Follow = followPos;
        idleCam.enabled = false;
        followCam.enabled = true;
    }
}
