using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject MusicAndSoundMenu;
    [SerializeField] private ManagerData managerData;
    private bool gamePaused = false;
    private GameMaster gm;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Rotation rotation;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Manual manual;
    private bool inmortality = false;
    public Toggle toggle;
    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        gamePaused = false;
        managerData = FindObjectOfType<ManagerData>();
        if (managerData == null)
        {
            //Debug.LogError("No se encontr� una instancia de ManagerData en la escena.");
        }

        toggle.isOn = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        //Debug.Log("Pausa");
        gamePaused = true;
        Time.timeScale = 0f;
        manual.pageIndex = 4;
        manual.Show();
        manual.showMenu();
        rotation.canShoot = false;
    }

    public void Resume()
    {
        //Debug.Log("Reanudar");
        gamePaused = false;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
        MusicAndSoundMenu.SetActive(false);
        rotation.canShoot = true;
        manual.Hide();
        manual.pageIndex = 0;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        //Debug.Log("Salir al menu");
        managerData.ResetGameData();
        SceneManager.LoadScene("Menu");
        gm.lastCheckpoint = Vector2.zero;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        //Debug.Log("reset");
        managerData.ResetGameData();
        SceneManager.LoadScene("PacificZone");
        gm.lastCheckpoint = Vector2.zero;
    }

    public void Inmortality()
    {
        inmortality = !inmortality; 

        if (inmortality)
        {
            playerHealth.health = 3000;
            toggle.isOn = true;
            Debug.Log("Inmortalidad activada ");
        }
        else
        {
            playerHealth.health = 4;
            toggle.isOn = false;
            Debug.Log("Inmortalidad desactivada ");
        }
    }

    public void MusicAndSound()
    {
        MusicAndSoundMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        MusicAndSoundMenu.SetActive(false);
    }
}
