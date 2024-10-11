using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CamTransition : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private CinemachineVirtualCamera shrineCamera;

    [SerializeField] private BossMachine boss;
    [SerializeField] AudioManager audioManager;
    public IEnumerator SwitchPriorityShrine()
    {
        if (virtualCamera)
        {
            virtualCamera.Priority = 0;
            shrineCamera.Priority = 1;
            Debug.Log("portalCamera");
        }

        yield return new WaitForSeconds(4);
        virtualCamera.Priority = 1;
        shrineCamera.Priority = 0;
    }

    public IEnumerator SwitchPriorityBoss()
    {
        if (virtualCamera)
        {
            virtualCamera.Priority = 0;
            bossCamera.Priority = 1;
            audioManager.playSound(audioManager.bossAwaken);
            audioManager.ChangeBackgroundMusic(audioManager.bossMusic);
            Debug.Log("bossCamera");
        }

        yield return new WaitForSeconds(4);
        virtualCamera.Priority = 1;
        bossCamera.Priority = 0;

        yield return new WaitForSeconds(2);
        boss.OnActive();
    }
}
