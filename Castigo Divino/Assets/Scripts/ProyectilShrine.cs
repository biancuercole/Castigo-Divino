using System.Collections;
using UnityEngine;

public class ProyectileShrine : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    [SerializeField] private TrailRenderer trail;
    private  GameObject shrine; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shrine = GameObject.Find("Shrine");
        LaunchProyectile();
    }

    private void LaunchProyectile()
    {
        Vector2 directionToShrine = (shrine.transform.position - transform.position).normalized;
        rb.velocity = directionToShrine * speed;
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
       
            ShrineHealth shrineHealth = collision.gameObject.GetComponent<ShrineHealth>();
            if (shrineHealth != null)
            {
                Debug.Log("Se llama a GetDamage en " + collision.gameObject.name);
                StartCoroutine(shrineHealth.GetDamage(damage));
            }
            else
            {
                Debug.Log("No se encontró ShrineHealth en " + collision.gameObject.name);
            }
       
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
               playerHealth.GetDamage(damage);
            }



            Destroy(gameObject);
    }
}
