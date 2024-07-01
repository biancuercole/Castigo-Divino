using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float damage;
    [SerializeField] private GameObject Coin;
    [SerializeField] private TrailRenderer trail;

    private Rigidbody2D bulletRb;
    private float destroyDelay = 2f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletRb = GetComponent<Rigidbody2D>();
    }

    public void LaunchBullet(Vector2 direction)
    {
        audioManager.playSound(audioManager.shot);
        bulletRb.velocity = direction  * speed;
        Destroy(gameObject, destroyDelay);
        trail.emitting = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Proyectil"))
        {
            // Ignorar colisión con objetos que tengan el tag "bala"
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Machine"))
        {
            // Destruir la máquina
            Destroy(collision.gameObject);
        }

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // Llama al método TakeDamage
        }

        Destroy(gameObject);
    }
}

