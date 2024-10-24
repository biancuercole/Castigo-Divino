using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con imágenes en UI

public class Manual : MonoBehaviour
{
    private bool isManualShowing;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private Image manualImage; // Cambiamos TMP_Text por Image
    [SerializeField] private Sprite[] manualImages; // Array de imágenes tipo Sprite
    [SerializeField] private Rotation rotation;
    private int pageIndex;
    [SerializeField] private GameObject map;

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
    }

    public void showPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < manualImages.Length)
        {
            manualImage.sprite = manualImages[pageIndex]; // Cambiamos la imagen
        }
        map.SetActive(false);
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
            pageIndex = 0; // Vuelve al inicio si supera el número de imágenes
        }
        showPage(pageIndex);
    }

    public void PreviousPage()
    {
        pageIndex--;
        if (pageIndex < 0)
        {
            pageIndex = manualImages.Length - 1; // Vuelve a la última imagen si retrocede demasiado
        }
        showPage(pageIndex);
    }
}
