using UnityEngine;
using UnityEngine.UI;

/*public class Rotation : MonoBehaviour
{
    public enum BulletType { Celeste, Roja, Verde }

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private bool tripleShotEnabled = false;
    [SerializeField] private ManagerData managerData;
    private Camera cam;
    private float lastShootTime;
    public bool canShoot = true;
    private Animator animator;
    [SerializeField] private float shootAnimationDuration;
    [SerializeField] private Image bulletColorUI;

  //  public BulletType selectedBulletType = BulletType.Celeste;

    void Start()
    {
        cam = Camera.main;
        lastShootTime = -shootCooldown;
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>();

     //   UpdateBulletUIColor();
    }

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBulletType = BulletType.Celeste;
            UpdateBulletUIColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBulletType = BulletType.Roja;
            UpdateBulletUIColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBulletType = BulletType.Verde;
            UpdateBulletUIColor();
        }

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

   /* private void UpdateBulletUIColor()
    {
        switch (selectedBulletType)
        {
            case BulletType.Celeste:
                bulletColorUI.color = Color.cyan;
                break;
            case BulletType.Roja:
                bulletColorUI.color = Color.red;
                break;
            case BulletType.Verde:
                bulletColorUI.color = Color.green;
                break;
        }
    }

    private void ShootSingleBullet(Vector2 direction, float angle)
    {
        GameObject bullet = BulletPool.Instance.RequestBullet();
        if (bullet != null)
        {
            animator.SetBool("Shooting", true);
            bullet.transform.position = shootPosition.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            bullet.GetComponent<Bullets>().LaunchBullet(direction);

           // SetBulletColor(bullet);

            Invoke("ResetShootingAnimation", shootAnimationDuration);
        }
        else
        {
            Debug.LogWarning("No bullet available to shoot");
        }
    }

    private void ShootMultipleBullets(Vector2 direction, float angle)
    {
        float[] angleOffsets = { 0f, 30f, -40f };

        foreach (float offset in angleOffsets)
        {
            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
                bullet.transform.position = shootPosition.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle + offset);

                Vector2 adjustedDirection = Quaternion.Euler(0, 0, offset) * direction;
                bullet.GetComponent<Bullets>().LaunchBullet(adjustedDirection);

              //  SetBulletColor(bullet);

              // Debug.Log($"Bullet spawned at {bullet.transform.position} with direction {adjustedDirection}");
            }
            else
            {
                Debug.LogWarning("No bullet available to shoot");
            }
        }

        animator.SetBool("Shooting", true);
        Invoke("ResetShootingAnimation", shootAnimationDuration);
    }

   /* private void SetBulletColor(GameObject bullet)
    {
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        switch (selectedBulletType)
        {
            case BulletType.Celeste:
                spriteRenderer.color = Color.cyan;
                break;
            case BulletType.Roja:
                spriteRenderer.color = Color.red;
                break;
            case BulletType.Verde:
                spriteRenderer.color = Color.green;
                break;
        }
    }

    private void ResetShootingAnimation()
    {
        animator.SetBool("Shooting", false);
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
}*/


public class Rotation : MonoBehaviour
{
    public BulletType selectedBulletType = BulletType.Fire;  

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private bool tripleShotEnabled = false;
    [SerializeField] private ManagerData managerData;
    private Camera cam;
    private float lastShootTime;
    public bool canShoot = true;
    private Animator animator;
    [SerializeField] private float shootAnimationDuration;
    [SerializeField] private Image bulletColorUI;

    void Start()
    {
        cam = Camera.main;
        lastShootTime = -shootCooldown;
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>();
        canShoot = true;
        UpdateBulletUIColor();
    }
    private void Update()
    {
        // Cambia el tipo de bala según la tecla presionada
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBulletType = BulletType.Water;
            UpdateBulletUIColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBulletType = BulletType.Fire;
            UpdateBulletUIColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBulletType = BulletType.Air;
            UpdateBulletUIColor();
        }

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
                animator.SetBool("Shooting", true);
                bullet.transform.position = shootPosition.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

                Bullets bulletComponent = bullet.GetComponent<Bullets>();
                bulletComponent.bulletType = selectedBulletType;
                Debug.Log($"Shooting bullet of type: {bulletComponent.bulletType}"); // Debugging
                bulletComponent.LaunchBullet(direction);

                Invoke("ResetShootingAnimation", shootAnimationDuration);
            }
            else
            {
                Debug.LogWarning("No bullet available to shoot");
            }
    }

    private void ShootMultipleBullets(Vector2 direction, float angle)
    {
        float[] angleOffsets = { 0f, 30f, -40f };

        foreach (float offset in angleOffsets)
        {
            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
                bullet.transform.position = shootPosition.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle + offset);

                // Configura el tipo de bala en el componente Bullets
                Bullets bulletComponent = bullet.GetComponent<Bullets>();
                bulletComponent.bulletType = selectedBulletType;

                Vector2 adjustedDirection = Quaternion.Euler(0, 0, offset) * direction;
                bulletComponent.LaunchBullet(adjustedDirection);
            }
            else
            {
                Debug.LogWarning("No bullet available to shoot");
            }
        }

        animator.SetBool("Shooting", true);
        Invoke("ResetShootingAnimation", shootAnimationDuration);
    }

    private void UpdateBulletUIColor()
    {
        switch (selectedBulletType)
        {
            case BulletType.Water:
                bulletColorUI.color = Color.blue;
                break;
            case BulletType.Fire:
                bulletColorUI.color = Color.red;
                break;
            case BulletType.Air:
                bulletColorUI.color = Color.green;
                break;
        }
    }

    private void ResetShootingAnimation()
    {
        animator.SetBool("Shooting", false);
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


