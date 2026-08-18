using System;
using Unity.Cinemachine;
using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Player;
    [SerializeField] private CinemachineVirtualCameraBase activeCamera;
    [SerializeField] private CinemachineBrain brain;
    [Header("Blend Settings")]
    [SerializeField] private bool doesItBlend;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doesItBlend == true)
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
