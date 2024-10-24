using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    private bool isManualShowing;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private Image manualImage;
    [SerializeField] private Sprite[] manualImages;
    [SerializeField] private Image[] enemyCollected;
    [SerializeField] private Rotation rotation;
    [SerializeField] private GameObject map;

    public int pageIndex;
    public List<int> defeatedEnemies = new List<int>(); // Lista para enemigos derrotados

    void Start()
    {
        manualPanel.SetActive(false);
        map.SetActive(false);
        isManualShowing = false;
        pageIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isManualShowing)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        isManualShowing = true;
        Time.timeScale = 0f;
        manualPanel.SetActive(true);
        rotation.canShoot = false;
        showPage(pageIndex);
        UpdateDefeatedEnemiesUI(); // Actualiza los enemigos al abrir el manual
    }

    public void Hide()
    {
        isManualShowing = false;
        Time.timeScale = 1f;
        manualPanel.SetActive(false);
        rotation.canShoot = true;
        map.SetActive(false);
    }

    public void showPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < manualImages.Length)
        {
            manualImage.sprite = manualImages[pageIndex];
        }

        // Solo actualiza los enemigos derrotados si estamos en la página 1
        if (pageIndex == 1)
        {
            UpdateDefeatedEnemiesUI();
        }
        else
        {
            HideDefeatedEnemies(); // Oculta los enemigos si no es la página 1
        }

        map.SetActive(false);
    }

    // Método para ocultar los sprites de enemigos cuando no estamos en la página 1
    private void HideDefeatedEnemies()
    {
        foreach (Image enemy in enemyCollected)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void ShowDefeatedEnemies(int enemyIndex)
    {
        if (enemyIndex >= 0 && enemyIndex < enemyCollected.Length)
        {
            defeatedEnemies.Add(enemyIndex); // Agrega el enemigo a la lista
        }
    }

    private void UpdateDefeatedEnemiesUI()
    {
        foreach (int enemyIndex in defeatedEnemies)
        {
            if (enemyIndex >= 0 && enemyIndex < enemyCollected.Length)
            {
                enemyCollected[enemyIndex].gameObject.SetActive(true); // Muestra el enemigo
            }
        }
    }

    public void showMap()
    {
        map.SetActive(true);
    }

    public void NextPage()
    {
        pageIndex++;
        if (pageIndex >= manualImages.Length)
        {
            pageIndex = 0;
        }
        showPage(pageIndex);
    }

    public void PreviousPage()
    {
        pageIndex--;
        if (pageIndex < 0)
        {
            pageIndex = manualImages.Length - 1;
        }
        showPage(pageIndex);
    }
}
