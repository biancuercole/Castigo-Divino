
using System.Collections;
using UnityEngine;
//using UnityEngine.WSA;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int damage;
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
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage);
        }


        Destroy(gameObject);
    }
}
