using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Rendering;

public class ManagerData : MonoBehaviour
{
    public static ManagerData Instance;
    public float points;
    public int health;
    public bool isTripleShotBought;
    public bool isBulletPowerUpCollected;
    public float speed = 25;
    public float speedBullet;
    public float damageBullet;

    [SerializeField] private Rotation rotation;
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

    private void Start()
    {
        if (points == 0 && health == 0)
        {
            LoadPoints();
        }

        isTripleShotBought = false;
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

    public void AddHealth(int currentHealth)
    {
        health = currentHealth;
        PlayerPrefs.SetInt("PlayerHealth", health);
    }

    public void AddSpeed(float Speed)
    {
      speed += Speed;
      PlayerPrefs.SetFloat("SpeedBuff", speed);
      PlayerPrefs.Save();
    }


    public void AddSpeedBullet(float SpeedBullet)
    {
        speedBullet += SpeedBullet;
        PlayerPrefs.SetFloat("BulletSpeed", speedBullet);
        PlayerPrefs.Save();
        Debug.Log("BulletSpeed=" + speedBullet);

    }
    public void AddDamageBullet(float DamageBullet) 
    {
        damageBullet += DamageBullet;
        PlayerPrefs.SetFloat("BulletDamage", damageBullet);
        PlayerPrefs.Save();
        Debug.Log("BulletDamage=" + damageBullet);
    }

    public void TakeSpeedBullet(float SpeedBullet)
    {
        speedBullet -= SpeedBullet;
        PlayerPrefs.SetFloat("BulletSpeed", speedBullet);
    }

    /* public void AddMaxHealth(int maxHealth)
     {
         PlayerPrefs.SetInt("PlayerMaxHealth", maxHealth);
     }*/

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

   /* public int LoadMaxHealth()
    {
        return PlayerPrefs.GetInt("PlayerMaxHealth", 4); // Por defecto, 4 corazones
    }*/
    public void LoadPoints()
    {
        points = PlayerPrefs.GetFloat("PlayerPoints", points);
        health = PlayerPrefs.GetInt("PlayerHealth", health);
        speed = PlayerPrefs.GetFloat("PlayerSpeed", speed);
        speedBullet = PlayerPrefs.GetFloat("BulletSpeed", speedBullet);
        damageBullet = PlayerPrefs.GetFloat("BulletDamage", damageBullet);
        Debug.Log("Datos cargados: Puntos=" + points + ", Salud=" + health + ", Velocidad=" + speed + ", BulletSpeed=" + speedBullet);

        if (isTripleShotBought)
        {
            ApplyBulletPowerUp();
        }
    }

    public void ApplyBulletPowerUp()
    {
        FindObjectOfType<Rotation>().EnableTripleShot();
    }

    public void ItemSetActive()
    {
        isBulletPowerUpCollected = true;
        Debug.Log("item activado");
    }
}
