
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float damage;
    [SerializeField] private GameObject powerUp;

    private Rigidbody2D bulletRb;
    private float destroyDelay = 2f;
    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    public void LaunchBullet(Vector2 direction)
    {
        bulletRb.velocity = direction * speed;
        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Machine"))
        {
            Instantiate(powerUp, collision.transform.position, Quaternion.identity);
            // Destruir la máquina
            Destroy(collision.gameObject);
        }

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            StartCoroutine(enemyHealth.GetDamage(damage));
        }
       
        Destroy(gameObject);

    }
}

