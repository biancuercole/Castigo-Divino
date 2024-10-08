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

    [SerializeField] public PowerOfGod powerUpBar;
    [SerializeField] private float Power;

    private AudioManager audioManager;

    public GameObject UIposition;
    private GameObject[] lootToUI;

    private void Start()
    {
        pointsUI = FindObjectOfType<PointsUI>();

        managerData = FindObjectOfType<ManagerData>();

        powerUpBar = FindObjectOfType<PowerOfGod>();

        audioManager = FindAnyObjectByType<AudioManager>();

        if (pointsUI == null)
        {
            //Debug.LogError("No se encontr� un componente PointsUI en la escena.");
        }

        if (playerHealth == null)
        {
            //Debug.LogError("No se encontr� un componente PlayerHealth en el jugador.");
        }

        if (powerUpBar == null)
        {
            //Debug.LogError("No se encontro powerUpBar en la escena.");
        }

        powerUpBar.ShowBar();
    }
    void Update()
    {
        lootToUI = GameObject.FindGameObjectsWithTag("powerLeaf");
        GameObject[] magnetizableObjects = GameObject.FindGameObjectsWithTag("coin");
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("heart"));
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("key"));
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("bulletPowerUp"));
        magnetizableObjects = CombineArrays(magnetizableObjects, GameObject.FindGameObjectsWithTag("powerLeaf"));

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
            //MagnetToUI();
            Destroy(other.gameObject);
            pointsUI.TakePoints(amountPoints);
            audioManager.playSound(audioManager.collectSound);
        }
        if (other.gameObject.CompareTag("heart"))
        {
            //Debug.Log("Corazon");
            if (playerHealth != null)
            {
                Destroy(other.gameObject);
                //Debug.Log("Recolectado: " + loot.lootName);
                playerHealth.HealHealth(healAmount);
            }
        }
        if (other.gameObject.CompareTag("bulletPowerUp"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PowerUpUnlocked());
            audioManager.playSound(audioManager.healSound);
        }

        if (other.gameObject.CompareTag("powerLeaf"))
        {
            MagnetToUI();
            audioManager.playSound(audioManager.collectSound);
        }

        if (other.gameObject.CompareTag("key"))
        {
            GameEvents.KeyCollected();
            Destroy(other.gameObject);
            audioManager.playSound(audioManager.collectSound);
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

    private Vector3 GetUIWorldPosition()
    {
        RectTransform uiRectTransform = UIposition.GetComponent<RectTransform>();
        Vector3 uiScreenPosition = uiRectTransform.position;
        Vector3 uiWorldPosition = Camera.main.ScreenToWorldPoint(uiScreenPosition);
        uiWorldPosition.z = 0; 
        return uiWorldPosition;
    }


    private void MagnetToUI()
    {
        foreach (GameObject obj in lootToUI)
        {
            StartCoroutine(MoveToUI(obj));
        }
    }


    private IEnumerator MoveToUI(GameObject loot)
    {
        float moveSpeed = 60f;
        Rigidbody2D rb = loot.GetComponent<Rigidbody2D>();

        if (rb == null) yield break;

        while (Vector2.Distance(loot.transform.position, GetUIWorldPosition()) > 0.5f)
        {
            Vector3 targetWorldPosition = GetUIWorldPosition();
            Vector2 targetDirection = (targetWorldPosition - loot.transform.position).normalized;
            rb.velocity = targetDirection * moveSpeed;
            yield return null; 
        }

        Destroy(loot);
        powerUpBar.TakePower(Power);
    }
}