using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSequence : MonoBehaviour
{
    public Sprite[] images; // Array para almacenar las imágenes
    private int currentImageIndex = 0; // Índice de la imagen actual
    private Image displayImage; // Componente Image para mostrar la imagen
    [SerializeField] private int levelIndex;

    void Start()
    {
        displayImage = GetComponent<Image>(); // Obtener el componente Image
        if (images.Length > 0)
        {
            displayImage.sprite = images[currentImageIndex]; // Mostrar la primera imagen
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ) // Detectar si se presiona Enter
        {
            ShowNextImage(); // Mostrar la siguiente imagen
        }
    }

    void ShowNextImage()
    {
        currentImageIndex++; // Incrementar el índice
        if (currentImageIndex < images.Length)
        {
            displayImage.sprite = images[currentImageIndex]; // Cambiar a la siguiente imagen
        }
        else
        {
            changeLevel(levelIndex);
        }
    }

    public void changeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}