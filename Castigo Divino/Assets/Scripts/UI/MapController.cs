using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float minFOV = 20f; // Valor m�nimo del Field of View
    [SerializeField] private float maxFOV = 178f; // Valor m�ximo del Field of View
    [SerializeField] private float zoomSpeed = 10f; // Velocidad del zoom
    [SerializeField] private float dragSpeed = 500f; // Velocidad del arrastre de la c�mara

    private Vector3 dragOrigin;

    void Start()
    {
        cam.fieldOfView = maxFOV; // Establecer el Field of View inicial en el valor m�ximo
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
            // Limita el Field of View entre los valores m�nimos y m�ximos
            cam.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }

    private void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta cuando se presiona el bot�n izquierdo del mouse
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return; // Si no se mantiene presionado el bot�n izquierdo, no hace nada

        // Calcula la diferencia entre la posici�n actual del mouse y la posici�n de arrastre original
        Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        // Mueve la c�mara
        cam.transform.Translate(-move, Space.World);

        // Actualiza el origen de arrastre
        dragOrigin = Input.mousePosition;
    }
}
