using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject menuPause;

    private bool gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("Pausa");
        gamePaused = true;
        Time.timeScale = 0f;
        menuPause.SetActive(true);
    }

    public void Resume()
    {
        Debug.Log("Reanudar");
        gamePaused = false;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
    }

    public void Menu()
    {
        Debug.Log("Salir al menu");
        SceneManager.LoadScene("Menu");
    }


}
