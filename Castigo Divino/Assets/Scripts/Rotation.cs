using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootCooldown = 1f; // Cooldown de 1 segundo
    [SerializeField] private bool tripleShotEnabled = false;
    [SerializeField] private ManagerData managerData;
    private Camera cam;
    private float lastShootTime; // Tiempo del último disparo
    public bool canShoot = true;
    private Animator animator; // Referencia al Animator
    [SerializeField] private float shootAnimationDuration; // Duración de la animación de disparo

    void Start()
    {
        cam = Camera.main;
        lastShootTime = -shootCooldown; // Permite disparar inmediatamente al iniciar
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>(); // Obtén el componente Animator
    }

    void Update()
    {
        Vector2 mouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPoint - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + shootCooldown && canShoot)
        {
            lastShootTime = Time.time;

            if (tripleShotEnabled)
            {
                ShootMultipleBullets(direction, angle);
            }
            else
            {
                ShootSingleBullet(direction, angle);
            }
        }
    }

    private void ShootSingleBullet(Vector2 direction, float angle)
    {
        GameObject bullet = BulletPool.Instance.RequestBullet();
        if (bullet != null)
        {
            animator.SetBool("Shooting", true); // Activa la animación de disparo
            bullet.transform.position = shootPosition.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            bullet.GetComponent<Bullets>().LaunchBullet(direction);

            Invoke("ResetShootingAnimation", shootAnimationDuration); // Resetea la animación después de la duración
        }
        else
        {
            Debug.LogWarning("No bullet available to shoot");
        }
    }

    private void ShootMultipleBullets(Vector2 direction, float angle)
    {
        float[] angleOffsets = {0f, 30f, -30f}; // Desplazamientos de ángulo para las tres balas

        foreach (float offset in angleOffsets)
        {
            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
                bullet.transform.position = shootPosition.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle + offset);

                Vector2 adjustedDirection = Quaternion.Euler(0, 0, offset) * direction;
                bullet.GetComponent<Bullets>().LaunchBullet(adjustedDirection);

                Debug.Log($"Bullet spawned at {bullet.transform.position} with direction {adjustedDirection}");
            }
            else
            {
                Debug.LogWarning("No bullet available to shoot");
            }
        }

        animator.SetBool("Shooting", true); // Activa la animación de disparo
        Invoke("ResetShootingAnimation", shootAnimationDuration); // Resetea la animación después de la duración
    }

    private void ResetShootingAnimation()
    {
        animator.SetBool("Shooting", false); // Desactiva la animación de disparo
    }

    public void EnableTripleShot()
    {
        tripleShotEnabled = true;
        managerData.isTripleShotBought = true;
    }

    public void DisableTripleShot()
    {
        tripleShotEnabled = false;
        managerData.isTripleShotBought = false;
    }
}
