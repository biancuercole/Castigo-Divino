using System.Collections;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    [SerializeField] private TrailRenderer trail;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();

        LaunchProyectile();
    }

    private void LaunchProyectile()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        rb.velocity = directionToPlayer * speed;
        trail.emitting = true;
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        float destroyTime = 5f;
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            // Ignorar colisión con objetos que tengan el tag "bala"
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage);
        }

        Destroy(gameObject);
    }
}
