using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionEscena : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacionFinal;
    private ManagerData managerData;
    // Start is called before the first frame update
    void Start()
    {
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SiguienteNivel (string sceneName)
    {
        StartCoroutine(CambiarEscena(sceneName));
    }

    public IEnumerator CambiarEscena(string sceneName)
    {
        Time.timeScale = 1;
        animator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(animacionFinal.length);
        SceneManager.LoadScene(sceneName);
    }

    public void CambiarNivel(int indice)
    {
        if (managerData != null)
        {
            managerData.ResetGameData();
        }
        else
        {
            //.LogError("ManagerData no está asignado.");
        }
        managerData.ResetGameData();
        SceneManager.LoadScene(indice);
    }
}
