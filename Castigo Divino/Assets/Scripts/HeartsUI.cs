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
       /* healthPlayer.changeMaxHealth.AddListener(UpdateMaxHealth);*/
    }

    public void changeHeart(int health)
    {
        if (!listHearts.Any())
        {
            createHeart(health);
        }
        else
        {
            changeHealth(health);
        }
    }
    public void createHeart(int AmountMaxhealth)
    {
        for (int i = 0; i < AmountMaxhealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);

            listHearts.Add(heart.GetComponent<Image>());
        }

        indexActual = AmountMaxhealth - 1;
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

    /*public void UpdateMaxHealth(int maxHealth)
    {
        createHeart(maxHealth);  // Crear corazones adicionales si es necesario
    }*/

}


