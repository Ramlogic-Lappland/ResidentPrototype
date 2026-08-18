using System;
using Unity.Cinemachine;
using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private CinemachineVirtualCameraBase activeCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCamera.Priority = 1;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCamera.Priority = 0;
        }
    }
}
