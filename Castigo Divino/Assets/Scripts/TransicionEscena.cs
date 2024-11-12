using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicionEscena : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacionFinal;
    [SerializeField] private Slider barraProgreso;
    private ManagerData managerData;
    private float progresoSimulado = 0f;
    private const float VELOCIDAD_LLENADO = 1.5f; // Velocidad para llenar la barra más lentamente

    void Start()
    {
        managerData = FindObjectOfType<ManagerData>();
        animator = GetComponent<Animator>();
        barraProgreso.gameObject.SetActive(false);
    }

    public void SiguienteNivel(string sceneName)
    {
        StartCoroutine(CargarEscenaConTransicion(sceneName));
    }

    private IEnumerator CargarEscenaConTransicion(string sceneName)
    {
        Time.timeScale = 1;
        animator.SetTrigger("StartTransition");
        
        yield return new WaitForSeconds(animacionFinal.length);

        barraProgreso.gameObject.SetActive(true);
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(sceneName);
        operacionCarga.allowSceneActivation = false;

        while (!operacionCarga.isDone)
        {
            float progresoReal = Mathf.Clamp01(operacionCarga.progress / 0.9f);
            
            // Incrementa progresoSimulado más lentamente
            progresoSimulado = Mathf.Lerp(progresoSimulado, progresoReal, Time.deltaTime * VELOCIDAD_LLENADO);

            barraProgreso.value = progresoSimulado;

            // Cuando llega a 0.9, espera a que la barra se llene al 100% antes de activar la escena
            if (progresoReal >= 0.9f && progresoSimulado >= 0.99f)
            {
                operacionCarga.allowSceneActivation = true;
            }

            yield return null;
        }

        barraProgreso.gameObject.SetActive(false);
    }

    public void CambiarNivel(string sceneName)
    {
        if (managerData != null)
        {
            managerData.ResetGameData();
        }
        else
        {
            Debug.LogError("ManagerData no está asignado.");
        }
        StartCoroutine(CargarEscenaConTransicion(sceneName));
    }
}
