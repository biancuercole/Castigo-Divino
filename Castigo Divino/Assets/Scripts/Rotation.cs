using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Bullets bulletPrefab;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform shootPosition;



    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        Vector2 mouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPoint - (Vector2)transform.position;

        // Calcular el ángulo y ajustar la rotación
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        if (Input.GetMouseButtonDown(0))
        {
            Bullets bullet = Instantiate(bulletPrefab, shootPosition.position, Quaternion.Euler(0, 0, angle));
            bullet.LaunchBullet(direction.normalized);
        }
    }
}
