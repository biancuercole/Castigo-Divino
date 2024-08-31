using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magnetStrength = 5f;  
    public float magnetRange = 5f;    

    [SerializeField] private int amountPoints;
    [SerializeField] private PointsUI pointsUI;
    [SerializeField] private Loot loot;

    [SerializeField] public GameObject newItem;

    [SerializeField] public PlayerHealth playerHealth;
    public int healAmount;

   private ManagerData managerData;
    private void Start()
    {
        pointsUI = FindObjectOfType<PointsUI>();

        managerData = FindObjectOfType<ManagerData>();

        if (pointsUI == null)
        {
            Debug.LogError("No se encontr� un componente PointsUI en la escena.");
        }

        if (playerHealth == null)
        {
            Debug.LogError("No se encontr� un componente PlayerHealth en el jugador.");
        }
    }
    void Update()
    {
       
        GameObject[] magnetizableObjects = GameObject.FindGameObjectsWithTag("coin");
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("heart"));
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("key"));
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("bulletPowerUp"));

        foreach (GameObject obj in magnetizableObjects)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);

           
            if (distance <= magnetRange)
            {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                   
                    Vector2 targetDirection = (transform.position - obj.transform.position).normalized;

                 
                    rb.velocity = targetDirection * magnetStrength;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
     if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            pointsUI.takePoints(amountPoints);
        }
        if (other.gameObject.CompareTag("heart"))
        {
           Debug.Log("Corazon");
            if (playerHealth != null)
            {
                Destroy(other.gameObject);
                Debug.Log("Recolectado: " + loot.lootName);
                playerHealth.HealHealth(healAmount);
            }
        }
        if (other.gameObject.CompareTag("bulletPowerUp"))
        {
            Destroy(other.gameObject);
           StartCoroutine(PowerUpUnlocked());
        }

        if (other.gameObject.CompareTag("altarVida"))
        {
            Debug.Log("salud recuperada");
            playerHealth.HealHealth(4);
        }
        if (other.gameObject.CompareTag("key"))
        {
            GameEvents.KeyCollected();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator PowerUpUnlocked()
    {
        managerData.ItemSetActive();
        newItem.SetActive(true);
        yield return new WaitForSeconds(5);
        newItem.SetActive(false);
    }

    private GameObject[] CombineArrays(GameObject[] array1, GameObject[] array2)
    {
        GameObject[] result = new GameObject[array1.Length + array2.Length];
        array1.CopyTo(result, 0);
        array2.CopyTo(result, array1.Length);
        return result;
    }
}
