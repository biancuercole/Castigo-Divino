
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
//using static UnityEditor.Progress;
using UnityEngine.UI;

public class ManagerData : MonoBehaviour
{
    public static ManagerData Instance;
    public int points;
    public int health;
    public bool isTripleShotBought;
    public bool isBulletPowerUpCollected;
    public bool isSpeedBulletBought;
    public bool isDamageBulletBought;
    public float speed = 25;
    public float speedBullet = 3;
    public float damageBullet = 1;
    public float CurrentDamageBonus = 0;
    public float CurrentSpeedBonus = 0;
    private GameMaster gm;
    public bool level1Finished = false;

    [SerializeField] private Rotation rotation;
    [SerializeField] private UIshop uiShop;


    private HashSet<string> boughtItems = new HashSet<string>();

    private Dictionary<string, Color> itemColors = new Dictionary<string, Color>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBoughtItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        if (points == 0 && health == 0)
        {
            LoadPoints();
        }

        isTripleShotBought = false;

        uiShop = FindObjectOfType<UIshop>();


        foreach (ItemSlot itemSlot in FindObjectsOfType<ItemSlot>())
        {
            GameObject item = itemSlot.gameObject;
            string itemName = item.name;

            // Cargar el color desde PlayerPrefs
            string colorString = PlayerPrefs.GetString(itemName + "_Color", null);
            if (!string.IsNullOrEmpty(colorString) && ColorUtility.TryParseHtmlString("#" + colorString, out Color color))
            {
                item.GetComponent<Image>().color = color;
            }
            Debug.Log(itemName + "_Color " + item.GetComponent<Image>().color + " managerData");
        }
    }

    public void ResetPoints()
    {
        points = points / 2;
        PlayerPrefs.SetInt("PlayerPoints", points);
    }


    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        PlayerPrefs.SetInt("PlayerPoints", points);
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
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("BulletSpeed", speedBullet);
    }

    public void TakeDamageBullet(float DamageBullet)
    {
        damageBullet -= DamageBullet;
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("BulletDamage", damageBullet);
    }

    /* public void AddMaxHealth(int maxHealth)
     {
         PlayerPrefs.SetInt("PlayerMaxHealth", maxHealth);
     }*/

    public bool SpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;
            PlayerPrefs.SetInt("PlayerPoints", points);
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
        points = PlayerPrefs.GetInt("PlayerPoints", points);
        health = PlayerPrefs.GetInt("PlayerHealth", health);
        speed = PlayerPrefs.GetFloat("PlayerSpeed", speed);
        speedBullet = PlayerPrefs.GetFloat("BulletSpeed", speedBullet);
        damageBullet = PlayerPrefs.GetFloat("BulletDamage", damageBullet);

        Debug.Log("Datos cargados: Puntos=" + points + ", Salud=" + health + ", Velocidad=" + speed +
            ", BulletSpeed=" + speedBullet + "_Color" + itemColors);

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

    public void IsBought()
    {
        isSpeedBulletBought = true;
        isDamageBulletBought = true;
    }

    public void MarkItemAsBought(string itemName)
    {
        if (!boughtItems.Contains(itemName))
        {
            boughtItems.Add(itemName);
            SaveBoughtItems();
        }
    }

    public bool IsItemBought(string itemName)
    {
        return boughtItems.Contains(itemName);
    }

    private void SaveBoughtItems()
    {
        string boughtItemsString = string.Join(",", boughtItems);
        PlayerPrefs.SetString("BoughtItems", boughtItemsString);
        PlayerPrefs.Save();
    }

    private void LoadBoughtItems()
    {
        string boughtItemsString = PlayerPrefs.GetString("BoughtItems", "");
        if (!string.IsNullOrEmpty(boughtItemsString))
        {
            boughtItems = new HashSet<string>(boughtItemsString.Split(','));
        }
    }

    public void ResetGameData()
    {
        if (uiShop != null)
        {
            uiShop.ResetShop();
        }
        else
        {
            Debug.LogError("uiShop no esta inicializado.");
        }

        // Reinicia otros valores a sus estados predeterminados
        points = 0;
        health = 4; // O el valor que consideres por defecto
        isTripleShotBought = false;
        isBulletPowerUpCollected = false;
        isSpeedBulletBought = false;
        isDamageBulletBought = false;
        speed = 25;
        speedBullet = 3;
        damageBullet = 1;
        CurrentDamageBonus = 0;
        CurrentSpeedBonus = 0;

        // Limpia los items comprados
        boughtItems.Clear();
        SaveBoughtItems();
    
        PlayerPrefs.DeleteAll();
        Debug.Log("Datos del juego reiniciados.");
    }
}
