using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float minFOV = 20f; // Valor mínimo del Field of View
    [SerializeField] private float maxFOV = 178f; // Valor máximo del Field of View
    [SerializeField] private float zoomSpeed = 10f; // Velocidad del zoom
    [SerializeField] private float dragSpeed = 500f; // Velocidad del arrastre de la cámara

    private Vector3 dragOrigin;

    void Start()
    {
        cam.fieldOfView = maxFOV; // Establecer el Field of View inicial en el valor máximo
    }

    void Update()
    {
        HandleZoom();
        HandleCameraDrag();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            // Calcula el nuevo valor de Field of View con un factor de zoom
            float newFOV = cam.fieldOfView - scroll * zoomSpeed;
            // Limita el Field of View entre los valores mínimos y máximos
            cam.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }

    private void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta cuando se presiona el botón izquierdo del mouse
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return; // Si no se mantiene presionado el botón izquierdo, no hace nada

        // Calcula la diferencia entre la posición actual del mouse y la posición de arrastre original
        Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        // Mueve la cámara
        cam.transform.Translate(-move, Space.World);

        // Actualiza el origen de arrastre
        dragOrigin = Input.mousePosition;
    }
}
