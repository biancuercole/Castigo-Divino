using System.Collections;
using UnityEngine;

public class Proyectiles: MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    [SerializeField] private TrailRenderer trail;
    private GameObject shrine;
    private Transform player;
    private Rigidbody2D rb;
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shrine = GameObject.Find("Shrine");

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        if (shrine == null)
        {
            shrine = GameObject.FindWithTag("Shrine");
        }

        LaunchProyectile();
    }

    public void LaunchProyectile()
    {
        audioManager.playSound(audioManager.enemyShot);
       // Debug.Log("Launching projectile. Object tag: " + this.gameObject.tag);

        if (this.gameObject.CompareTag("Proyectile"))
        {
           // Debug.Log("Targeting player");
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * speed;
        }
        else
        {
           // Debug.Log("Targeting shrine");
            Vector2 directionToShrine = (shrine.transform.position - transform.position).normalized;
            rb.velocity = directionToShrine * speed;
            
        }


        trail.emitting = true;
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        float destroyTime = 3f;
        yield return new WaitForSeconds(destroyTime);
     
            Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bala"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }

        ShrineHealth shrineHealth = collision.gameObject.GetComponent<ShrineHealth>();
        if (shrineHealth != null)
        {
           // Debug.Log("Se llama a GetDamage en " + collision.gameObject.name);
            StartCoroutine(shrineHealth.GetDamage(damage));
        }
        else
        {
           // Debug.Log("No se encontr? ShrineHealth en " + collision.gameObject.name);
        }

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage, this.gameObject);
        }

      
            Destroy(gameObject);
       
    }
}
