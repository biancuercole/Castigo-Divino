using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float yOffset;
    [SerializeField] private float xOffset;

    // Límites del mundo
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        // Calcular la mitad de la altura y el ancho de la cámara
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
    }

    void Update()
    {
        Vector3 newSpot = new Vector3(target.position.x + xOffset, target.position.y + yOffset, -10f);

        // Asegurarse de que la cámara se mantenga dentro de los límites del mundo
        float clampedX = Mathf.Clamp(newSpot.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(newSpot.y, minY + camHalfHeight, maxY - camHalfHeight);

        Vector3 clampedSpot = new Vector3(clampedX, clampedY, newSpot.z);

        transform.position = Vector3.Slerp(transform.position, clampedSpot, followSpeed * Time.deltaTime);
    }
}
