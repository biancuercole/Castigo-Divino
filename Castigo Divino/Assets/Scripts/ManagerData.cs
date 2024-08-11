using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerData : MonoBehaviour
{
    public static ManagerData Instance;
    public float points;

    private void Start()
    {
        ResetPoints();
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetPoints()
    {
        points = 0;
        PlayerPrefs.SetFloat("PlayerPoints", points);
    }

    public void AddPoints(float pointsToAdd)
    {
        points += pointsToAdd;
        PlayerPrefs.SetFloat("PlayerPoints", points);
    }

    public bool SpendPoints(float amount)
    {
        if (points >= amount)
        {
            points -= amount;
            PlayerPrefs.SetFloat("PlayerPoints", points);
            return true;
        }
        return false;
    }

    public void LoadPoints()
    {
        points = PlayerPrefs.GetFloat("PlayerPoints", 0);
    }
}
