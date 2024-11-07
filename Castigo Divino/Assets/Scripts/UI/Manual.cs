using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manual : MonoBehaviour
{
    private bool isManualShowing;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private Image manualImage;
    [SerializeField] private Sprite[] manualImages;
    [SerializeField] private Image[] enemyCollected;
    [SerializeField] private Rotation rotation;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI[] controlText; 

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
    }

    public void Hide()
    {
        isManualShowing = false;
        Time.timeScale = 1f;
        manualPanel.SetActive(false);
        rotation.canShoot = true;
        map.SetActive(false);
        menu.SetActive(false);
        pageIndex = 0;
    }

    public void showPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < manualImages.Length)
        {
            manualImage.sprite = manualImages[pageIndex];
        }

        if (pageIndex == 1) // Mostrar enemigos solo en la página 1
        {
            UpdateDefeatedEnemiesUI();
        }
        else
        {
            HideDefeatedEnemies();
        }

    if (pageIndex == 1) // Mostrar enemigos solo en la página 1
    {
        UpdateDefeatedEnemiesUI();
    }
    else
    {
        HideDefeatedEnemies();
    }

    // Mostrar todos los textos de control si pageIndex es 0
    if (pageIndex == 0)
    {
        Debug.Log("controles");
        foreach (TextMeshProUGUI text in controlText)
        {
            text.gameObject.SetActive(true); // Habilitar todos los elementos de controlText
        }
    }
    else
    {
        foreach (TextMeshProUGUI text in controlText)
        {
            text.gameObject.SetActive(false); // Deshabilitar si no es la página 0
        }
    }

        map.SetActive(false);
        menu.SetActive(false);
    }

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
            ManagerData.Instance.AddDefeatedEnemy(enemyIndex);
        }
    }

    private void UpdateDefeatedEnemiesUI()
    {
        foreach (int enemyIndex in ManagerData.Instance.defeatedEnemies)
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

    public void showMenu()
    {
        menu.SetActive(true);
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
