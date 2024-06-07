
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float damage;

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
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            StartCoroutine(enemyHealth.GetDamage(damage));
        }

       
        Destroy(gameObject);

    }
}

