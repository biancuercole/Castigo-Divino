using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;
    public TextMeshProUGUI textComponent;
    public float showTimer;
    private bool isItemTooltipActive = false; // Flag to track item tooltip state
    [SerializeField] private GameObject position;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isItemTooltipActive) return; // Skip timer if item tooltip is active

        transform.position = Input.mousePosition;
        showTimer -= Time.deltaTime;
        if (showTimer <= 0)
        {
            HideToolTip();
        }
    }

    public void SetAndShowToolTip(string message, float showTimerMax = 1.5f)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        showTimer = showTimerMax;
        Update();
    }

    public void ShowToolTip(string message, float showTimerMax = 4f)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        showTimer = showTimerMax;
        Update();
    }

    public void ShowItemToolTip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        isItemTooltipActive = true;
        transform.position = Input.mousePosition;
    }
    public void ShowTriggerToolTip(string message, Vector2 screenPosition)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        RectTransform canvasRectTransform = transform.parent.GetComponent<RectTransform>();

        // Verificar si el canvas es Screen Space - Overlay
        if (canvasRectTransform.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Si el Canvas está en Screen Space - Overlay, usa las coordenadas de pantalla directamente
            transform.position = screenPosition;
        }
        else
        {
            // Si el Canvas está en Screen Space - Camera o World Space, convierte las coordenadas
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                screenPosition,
                Camera.main,  // La cámara utilizada para la conversión
                out localPosition
            );

            // Establecer la posición local dentro del RectTransform
            GetComponent<RectTransform>().anchoredPosition = localPosition;
        }

        isItemTooltipActive = true;
        Debug.Log("ToolTipShowManager");
    
}
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
        isItemTooltipActive = false;
    }
}
