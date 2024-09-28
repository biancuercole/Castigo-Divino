using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manual : MonoBehaviour
{
    private bool isManualShowing;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private TMP_Text manualText;
    [SerializeField, TextArea(4, 6)] private string[] manualLines;
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
        Debug.Log("Manual");
        isManualShowing = true;
        Time.timeScale = 0f;
        manualPanel.SetActive(true);
        rotation.canShoot = false;
        showPage(pageIndex);
    }

    public void Hide()
    {
        Debug.Log("Reanudar");
        isManualShowing = false;
        Time.timeScale = 1f;
        manualPanel.SetActive(false);
        rotation.canShoot = true;
        map.SetActive(false);
    }

    public void showPage(int pageIndex)
    {
        manualText.text = string.Empty;
        foreach (char ch in manualLines[pageIndex])
        {
            manualText.text += ch;
        }
        map.SetActive(false);
    }

    public void showMap()
    {
        Debug.Log("Llamada a showMap() con índice: " + pageIndex);
        map.SetActive(true);
        Debug.Log("mapa abierto en indice 3");
    }

    /*public void NextPage(int pageIndex)
    {
        pageIndex++;
        if(pageIndex < manualLines.Length)
        {
            showPage();
        } else
        {
            pageIndex = 0;
            showPage();
        }
    }*/

    /*public void PreviousPage()
    {
        pageIndex--;
        if(pageIndex >= 0)
        {
            showPage();
        }else 
        {
            pageIndex = 0;
            showPage();
        }
    }*/
}
