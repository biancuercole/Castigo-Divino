//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
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

        // Calcular el angulo y ajustar la rotacion
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        if (Input.GetMouseButtonDown(0))
        {


            GameObject bullet = BulletPool.Instance.RequestBullet();
            if (bullet != null)
            {
             // Debug.Log("Bullet activated, setting position and rotation");
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
