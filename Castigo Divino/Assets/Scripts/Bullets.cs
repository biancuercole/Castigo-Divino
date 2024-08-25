using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] private TrailRenderer trail;
    private Rigidbody2D bulletRb;
    private NextStage nextStage; 
    private AudioManager audioManager;
    // private LootBag lootBag;

    public ManagerData managerData;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletRb = GetComponent<Rigidbody2D>();
       // lootBag = FindObjectOfType<LootBag>();
    }


    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
        managerData = FindObjectOfType<ManagerData>();
        managerData = ManagerData.Instance;
        if (managerData == null)
        {
            Debug.LogError("No se encontró ManagerData en la escena.");
            return;
        }

        speed = managerData.speedBullet;
        damage = managerData.damageBullet;
    }

    public void LaunchBullet(Vector2 direction)
    {
       // Debug.Log($"Launching bullet with direction: {direction.normalized} and speed: {speed}");
        bulletRb.velocity = direction * speed;
        trail.emitting = true;
        audioManager.playSound(audioManager.shot);
        StartCoroutine(DestroyProjectile());

      //  Debug.Log($"Bullet velocity after launch: {bulletRb.velocity}");
    }

    IEnumerator DestroyProjectile()
    {
        float destroyTime = 3f;
        yield return new WaitForSeconds(destroyTime);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        bulletRb.velocity = Vector2.zero;  // Reset velocity on enable
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Proyectile"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // Llama al método TakeDamage
        }
        BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage); // Llama al método TakeDamage
        }

        MachineHealth machineHealth = collision.gameObject.GetComponent<MachineHealth>();
        if (machineHealth != null)
        {
            machineHealth.machineDamage(); // Llama al método TakeDamage
        }

        if (collision.gameObject.CompareTag("Bala"))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            trail.emitting = false;
        }
      
    }
}
