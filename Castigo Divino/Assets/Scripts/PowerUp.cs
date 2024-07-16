using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
         powerUpEffect.Apply(collision.gameObject);
         Destroy(gameObject);
        
    }
}
