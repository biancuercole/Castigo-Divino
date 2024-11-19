using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite airSprite;
    [SerializeField] private Sprite earthSprite;

    private float lastScrollTime;
    [SerializeField] private float scrollCooldown = 0.1f; 
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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedBulletType = BulletType.Earth;
            UpdateBulletUIColor();
        }

        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Time.time >= lastScrollTime + scrollCooldown)
        {
            if (scroll > 0f)
            {
                CycleBulletType(1);
                lastScrollTime = Time.time;
            }
            else if (scroll < 0f)
            {
                CycleBulletType(-1);
                lastScrollTime = Time.time;
            }
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
            bulletComponent.LaunchBullet(direction);

            Invoke("ResetShootingAnimation", shootAnimationDuration);
        }
    }

    private void ShootMultipleBullets(Vector2 direction, float angle)
    {
        float[] angleOffsets = { 0f, 30f, -30f };
        float bulletSpacing = 0.5f; // La distancia entre las balas
        for (int i = 0; i < angleOffsets.Length; i++)
        {
            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
                Vector3 bulletPositionOffset = shootPosition.right * (i - 1) * bulletSpacing;
                bullet.transform.position = shootPosition.position + bulletPositionOffset;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle + angleOffsets[i]);

                Vector2 adjustedDirection = Quaternion.Euler(0, 0, angleOffsets[i]) * direction;
                Bullets bulletComponent = bullet.GetComponent<Bullets>();
                bulletComponent.bulletType = selectedBulletType;
                bulletComponent.LaunchBullet(adjustedDirection);
            }
        }

        animator.SetBool("Shooting", true);
        Invoke("ResetShootingAnimation", shootAnimationDuration);
    }

    private void CycleBulletType(int direction)
    {
        int bulletCount = System.Enum.GetValues(typeof(BulletType)).Length;
        int newBulletIndex = (int)selectedBulletType;

        do
        {
            newBulletIndex = (newBulletIndex + direction + bulletCount) % bulletCount;
        } while ((BulletType)newBulletIndex == BulletType.GodPower);

        selectedBulletType = (BulletType)newBulletIndex;
        UpdateBulletUIColor();
    }

    private void UpdateBulletUIColor()
    {
        switch (selectedBulletType)
        {
            case BulletType.Water:
                bulletColorUI.sprite = waterSprite;
                break;
            case BulletType.Fire:
                bulletColorUI.sprite = fireSprite;
                break;
            case BulletType.Air:
                bulletColorUI.sprite = airSprite;
                break;
            case BulletType.Earth:
                bulletColorUI.sprite = earthSprite;
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
