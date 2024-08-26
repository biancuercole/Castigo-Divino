
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class ManagerData : MonoBehaviour
{
    public static ManagerData Instance;
    public float points;
    public int health;
    public bool isTripleShotBought;
    public bool isBulletPowerUpCollected;
    public bool isSpeedBulletBought;
    public bool isDamageBulletBought;
    public float speed = 25;
    public float speedBullet = 3;
    public float damageBullet = 1;

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

    private void ApplyActivePowerUps()
    {
        if (isSpeedBulletBought)
        {
            ApplySpeedBulletPowerUp();
        }

        if (isDamageBulletBought)
        {
            ApplyDamageBulletPowerUp();
        }

        // Aplica otros power-ups aquí si es necesario
    }

    private void ApplySpeedBulletPowerUp()
    {
        // Aplica la mejora de velocidad de bala aquí
        var bbSpeed = ScriptableObject.CreateInstance<BBspeed>();
        bbSpeed.Apply(null);  // Aplica el bono al personaje o balas
    }

    private void ApplyDamageBulletPowerUp()
    {
        // Aplica la mejora de daño de bala aquí
        var bulletBuffDamage = ScriptableObject.CreateInstance<BulletBuffDamage>();
        bulletBuffDamage.Apply(null);  // Aplica el bono al personaje o balas
    }

    public void ResetGameData()
    {
        // Inicializa los valores predeterminados
        points = 0;
        health = 4;
        speed = 25;
        speedBullet = 3;
        damageBullet = 1;

        // Restablece el estado de las mejoras
        isTripleShotBought = false;
        isBulletPowerUpCollected = false;
        isSpeedBulletBought = false;
        isDamageBulletBought = false;

        // Limpia los items comprados
        boughtItems.Clear();
        SaveBoughtItems();

        // Reaplicar las mejoras activas si las ha
        ApplyActivePowerUps();

        PlayerPrefs.DeleteAll();
        Debug.Log("Datos del juego reiniciados.");
    }
}
