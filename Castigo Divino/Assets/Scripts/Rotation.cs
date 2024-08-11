using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootCooldown = 1f; // Cooldown de 3 segundos

    private Camera cam;
    private float lastShootTime; // Tiempo del último disparo

    void Start()
    {
        cam = Camera.main;
        lastShootTime = -shootCooldown; // Permite disparar inmediatamente al iniciar
    }

    void Update()
    {
        Vector2 mouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPoint - (Vector2)transform.position;

        // Calcular el ángulo y ajustar la rotación
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Verificar si el tiempo actual es mayor al último tiempo de disparo más el cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + shootCooldown)
        {
            lastShootTime = Time.time; // Actualizar el tiempo del último disparo

            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
                bullet.transform.position = shootPosition.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                bullet.GetComponent<Bullets>().LaunchBullet(direction);
            }
            else
            {
                Debug.LogWarning("No bullet available to shoot");
            }
        }
    }
}
