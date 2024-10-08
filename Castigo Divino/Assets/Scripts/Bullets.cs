using UnityEngine;
using System.Collections;
using static Rotation;
using static UnityEngine.Rendering.DebugUI;

/*public class Bullets : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] private TrailRenderer trail;
    private Rigidbody2D bulletRb;
    private NextStage nextStage; 
    private AudioManager audioManager;
    public ManagerData managerData;
    private Transform bullet;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
        managerData = FindObjectOfType<ManagerData>();
        managerData = ManagerData.Instance;
        if (managerData == null)
        {
            //Debug.LogError("No se encontró ManagerData en la escena.");
            return;
        }

        speed = managerData.speedBullet;
        damage = managerData.damageBullet;
    }

    public void LaunchBullet(Vector2 direction)
    {
        bulletRb.velocity = direction * speed;
        trail.emitting = true;
        audioManager.playSound(audioManager.shot);
        StartCoroutine(DestroyProjectile());
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
}*/


public class Bullets : MonoBehaviour
{
    public BulletType bulletType;  // Usa el enum global

    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] private TrailRenderer trail;
    private Rigidbody2D bulletRb;
    private AudioManager audioManager;
    private NextStage nextStage;
    public ManagerData managerData;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
        managerData = FindObjectOfType<ManagerData>();
        managerData = ManagerData.Instance;
        if (managerData == null)
        {
            //.LogError("No se encontró ManagerData en la escena.");
            return;
        }

        speed = managerData.speedBullet;
        damage = managerData.damageBullet;
    }

    public void LaunchBullet(Vector2 direction)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (bulletType)
        {
            case BulletType.Water:
                spriteRenderer.color = Color.blue;
                trail.startColor = Color.blue;
                trail.endColor = new Color(0, 0, 0, 0);
                audioManager.playSound(audioManager.waterShot);
                break;
            case BulletType.Fire:
                spriteRenderer.color = Color.red;
                trail.startColor = Color.red;
                trail.endColor = new Color(0, 0, 0, 0);
                audioManager.playSound(audioManager.fireShot);
                break;
            case BulletType.Air:
                spriteRenderer.color = Color.white;
                trail.startColor = Color.white;
                trail.endColor = new Color(0, 0, 0, 0);
                audioManager.playSound(audioManager.airShot);
                break;
            case BulletType.Earth:
                spriteRenderer.color = new Color(0.65f, 0.33f, 0.0f);
                trail.startColor = new Color(0.65f, 0.33f, 0.0f);
                trail.endColor = new Color(0, 0, 0, 0);
                audioManager.playSound(audioManager.earthShot);
                break;
        }

        bulletRb.velocity = direction * speed;
        trail.emitting = true;
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        float destroyTime = 3f;
        yield return new WaitForSeconds(destroyTime);

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Detecta si el objeto colisionado es un enemigo que implemente IDamageType
        IDamageType enemy = collision.gameObject.GetComponent<IDamageType>();

        if (enemy != null)
        {
            // Pasa el tipo de bala y el daño al enemigo
            enemy.TakeDamage(damage, bulletType);
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

       /* BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage); // Llama al método TakeDamage
        }*/

        gameObject.SetActive(false);
        trail.emitting = false;
    }
}



