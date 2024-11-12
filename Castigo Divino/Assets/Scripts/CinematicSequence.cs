using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSequence : MonoBehaviour
{
    private TransicionEscena transition;
    
    void Start()
    {
        transition = FindObjectOfType<TransicionEscena>();
    }
    
    void OnAnimationEnd() 
    {
        Debug.Log("La animación ha terminado");
        transition.SiguienteNivel("PacificZone");
    }
}