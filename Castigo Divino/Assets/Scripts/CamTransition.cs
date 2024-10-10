using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransition : MonoBehaviour
{

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera[] portalCamera;
    void Start()
    {

       SwitchToCamera(playerCamera);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
      //  currentView = views[0];

            CinemachineVirtualCamera targetCamera = GetComponent<CinemachineVirtualCamera>();
            SwitchToCamera(targetCamera);
            Debug.Log("Transicion a altar");

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToCamera(playerCamera);
            Debug.Log("Transicion a player");
        }
    }
    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineVirtualCamera camera in portalCamera)
        {
            camera.enabled = camera == targetCamera;
        }
    }
}
