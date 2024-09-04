using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    [SerializeField] private bool pasarNivel;
    [SerializeField] private int indiceNivel;
    public float DamageBullet, SpeedBullet;
    [SerializeField] private ManagerData managerData;
    private Animator transition;

    private void Start()
    {
        transition = GetComponentInChildren<Animator>();
        /*if (managerData == null)
        {*/
            managerData = FindObjectOfType<ManagerData>();
      /*  }*/
    }
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            CambiarNivel(indiceNivel);
        }
        if (pasarNivel)
        {
            CambiarNivel(indiceNivel);
        }
    }

    /* public void CambiarNivel(int indice)
     {
         //managerData.ResetGameData();
         StartCoroutine(SceneLoad(indice));
     }

     public IEnumerator SceneLoad(int indice)
     {
         transition.SetTrigger("StartTransition");
         yield return new WaitForSeconds(1f);
         SceneManager.LoadScene(indice);
     }*/

    public void CambiarNivel(int indice)
    {
        if (managerData != null)
        {
            managerData.ResetGameData();
        }
        else
        {
            Debug.LogError("ManagerData no está asignado.");
        }
        managerData.ResetGameData();
        SceneManager.LoadScene(indice);
    }
}