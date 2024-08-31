using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        ManagerData.Instance.LoadPoints();
        UpdatePointsUI();
    }

    public void takePoints(int pointsNow)
    {
        ManagerData.Instance.AddPoints(pointsNow);
        UpdatePointsUI();
        Debug.Log("Puntos: " + ManagerData.Instance.points);
    }

    public bool SpendPoints(int amount)
    {
        if (ManagerData.Instance.SpendPoints(amount))
        {
            UpdatePointsUI();
            return true;
        }
        return false;
    }

    private void UpdatePointsUI()
    {
        textMesh.text = ManagerData.Instance.points.ToString("0");
    }

}
