using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Bullets bulletPrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float rotationSpeed;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        Vector2 mouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPoint - (Vector2)transform.position;
        transform.up = Vector2.MoveTowards(transform.up, direction, rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Bullets bullet = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
            bullet.LaunchBullet(transform.up);
        }
    }
}
