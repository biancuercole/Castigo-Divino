using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float damage;
    [SerializeField] private TrailRenderer trail;
    private Rigidbody2D bulletRb;
    private NextStage nextStage; 
    private AudioManager audioManager;
    private LootBag lootBag;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletRb = GetComponent<Rigidbody2D>();
        lootBag = FindObjectOfType<LootBag>();
    }

    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
    }

    public void LaunchBullet(Vector2 direction)
    {
        audioManager.playSound(audioManager.shot);
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

        MachineHealth machineHealth = collision.gameObject.GetComponent<MachineHealth>();
        if (machineHealth != null)
        {
            machineHealth.machineDamage(); // Llama al método TakeDamage
        }

        gameObject.SetActive(false);
        trail.emitting = false;
    }
}
