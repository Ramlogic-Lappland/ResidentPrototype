using System;
using Unity.Cinemachine;
using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private CinemachineVirtualCameraBase activeCamera;
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private bool doesItBlend;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doesItBlend)
            {
                brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseIn;
            }
            else
            {
                brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
            }
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
