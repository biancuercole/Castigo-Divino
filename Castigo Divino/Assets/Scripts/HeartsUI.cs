using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HeartsUI : MonoBehaviour
{
   public List<Image> listHearts;

    public GameObject heartPrefab;

    public PlayerHealth healthPlayer;

    public int indexActual;

    public Sprite heartFull;

    public Sprite heartEmpty;

    private void Awake()
    {
        healthPlayer.changeHealth.AddListener(changeHeart);
    }

    public void changeHeart(int health)
    {
        if (!listHearts.Any())
        {
            createHeart(health);
        }
        else
        {
            if (health > listHearts.Count)
            {
                createHeart(health - listHearts.Count);
            }
            changeHealth(health);
        }
    }
    public void createHeart(int additionalHearts)
    {
        for (int i = 0; i < additionalHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            listHearts.Add(heart.GetComponent<Image>());
        }

        indexActual = listHearts.Count - 1;
    }
    public void changeHealth(int health)
    {
        if(health <= indexActual)
        {
            takeHeart(health);
        }
        else
        {
            giveHeart(health);
        }
    }
    
    public void takeHeart(int health)
    {
        for (int i = indexActual; i >= health; i--)
        {
            indexActual = i;
            listHearts [indexActual].sprite = heartEmpty; 
        }
    }

    public void giveHeart(int health)
    {
        for (int i = indexActual; i < health; i++)
        {
            indexActual = i;
            listHearts [indexActual].sprite = heartFull;
        }
    }

}


