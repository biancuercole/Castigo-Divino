using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    private float points;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void takePoints(float pointsNow)
    {
       
            points += pointsNow;
            textMesh.text = points.ToString("0");
            Debug.Log("Puntos: " + points);
        
    }

    public bool SpendPoints(float amount)
    {
        if (points >= amount)
        {
            points -= amount;
            UpdatePointsUI();
            return true;
        }
        return false;
    }

    private void UpdatePointsUI()
    {
        textMesh.text = points.ToString("0");
    }

}
