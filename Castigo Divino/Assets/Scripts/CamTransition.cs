using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CamTransition : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private CinemachineVirtualCamera shrineCamera;

    [SerializeField] AudioManager audioManager;
    [SerializeField] private BossMachine boss;
    public Animator bossAnimator;
    public GameObject bossAnimationObject;
    public UnityEvent OnBegin, OnDone;
    public float timeForAnimationJump;
    public float timeForCamPlayer;
    public float timeForPlayerToMove;

    public IEnumerator SwitchPriorityShrine()
    {
        if (virtualCamera)
        {
            virtualCamera.Priority = 0;
            shrineCamera.Priority = 1;
            Debug.Log("portalCamera");
        }

        yield return new WaitForSeconds(3);
        virtualCamera.Priority = 1;
        shrineCamera.Priority = 0;
    }

    public IEnumerator SwitchPriorityBoss()
    {
        if (virtualCamera)
        {
            OnBegin?.Invoke();
            virtualCamera.Priority = 0;
            bossCamera.Priority = 1;
          //  audioManager.playSound(audioManager.bossAwaken);

            yield return new WaitForSeconds(timeForAnimationJump);
            bossAnimationObject.SetActive(true);
            bossAnimator.SetTrigger("Jumping");
            Debug.Log("Jumping state after setting: " + bossAnimator.GetBool("Jumping"));

            // Esperar a que termine la animaci�n
          //  yield return new WaitUntil(() => IsAnimationFinished("Jumping"));

            // Cambiar la m�sica despu�s del salto
            audioManager.ChangeBackgroundMusic(audioManager.bossMusic);
            Debug.Log("bossCamera");

            // Resetear el trigger despu�s de que termine la animaci�n
            bossAnimator.ResetTrigger("Jumping"); 
        }

        yield return new WaitForSeconds(timeForCamPlayer);
        virtualCamera.Priority = 1;
        bossCamera.Priority = 0;
        bossAnimationObject.SetActive(false);
        boss.OnActive();        
      //  bossAnimationObject.SetActive(false);
        yield return new WaitForSeconds(timeForPlayerToMove);
        OnDone?.Invoke();
        Debug.Log("Door Closed ");
    }
}
