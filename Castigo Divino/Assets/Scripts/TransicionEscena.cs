using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicionEscena : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacionFinal;
    [SerializeField] private Slider barraProgreso; // Referencia al Slider de progreso
    private ManagerData managerData;
    private float progresoSimulado = 0f; // Progreso simulado para la barra

    void Start()
    {
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>();
        barraProgreso.gameObject.SetActive(false); // Oculta la barra al inicio
    }

    public void SiguienteNivel(string sceneName)
    {
        StartCoroutine(CargarEscenaConTransicion(sceneName));
    }

    private IEnumerator CargarEscenaConTransicion(string sceneName)
    {
        Time.timeScale = 1;
        animator.SetTrigger("StartTransition");
        
        // Espera a que termine la animación antes de iniciar la carga
        yield return new WaitForSeconds(animacionFinal.length);

        // Activa la barra de progreso y comienza a cargar la escena
        barraProgreso.gameObject.SetActive(true);
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(sceneName);
        operacionCarga.allowSceneActivation = false;

        // Actualiza el progreso de la barra mientras carga
        while (!operacionCarga.isDone)
        {
            // Progreso real limitado al 0.9
            float progresoReal = Mathf.Clamp01(operacionCarga.progress / 0.9f);
            
            // Usamos Lerp para incrementar progresoSimulado de forma gradual hasta alcanzar el progreso real
            progresoSimulado = Mathf.Lerp(progresoSimulado, progresoReal, Time.deltaTime * 3);

            // Asignamos el valor simulado al Slider
            barraProgreso.value = progresoSimulado;

            // Cuando llega al 0.9, habilitamos la activación de la escena
            if (progresoReal >= 0.9f)
            {
                operacionCarga.allowSceneActivation = true;
            }

            yield return null;
        }

        // Oculta la barra de progreso cuando la carga haya finalizado
        barraProgreso.gameObject.SetActive(false);
    }

    public void CambiarNivel(int indice)
    {
        if (managerData != null)
        {
            managerData.ResetGameData();
        }
        else
        {
            Debug.LogError("ManagerData no está asignado.");
        }
        StartCoroutine(CargarEscenaConTransicion(SceneManager.GetSceneByBuildIndex(indice).name));
    }
}
