using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dissolve : MonoBehaviour
{
    /* [SerializeField] private Material material;
     private float dissolveAmount;
     private bool isDissolving;

     private void Update()
     {
         if (isDissolving)
         {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
            material.SetFloat("DissolveAmount", dissolveAmount); 
         }

         if (Input.GetKeyDown(KeyCode.F))
         {
             isDissolving = true;
             Debug.Log("Dissolving");
         }
     }*/

   /* [SerializeField] private float dissolveTime = 0.75f;

    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;

    private int dissolveAmount = Shader.PropertyToID("DissolveAmount");
    private int verticalDissolveAmount = Shader.PropertyToID("VerticalDissolveAmount");

    private bool isDissolving;

    private void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }

    private void Update()
    {
        if (isDissolving)
        {
            float elapsedTime = 0f;
            while (elapsedTime < dissolveTime)
            {
                elapsedTime = Time.deltaTime;
                float lerpedDissolve = Mathf.Lerp(0, 1f, (elapsedTime / dissolveTime));
                float lerpedVerticalDissolve = Mathf.Lerp(0f, 1.1f, (elapsedTime / dissolveTime));

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat(dissolveAmount, lerpedDissolve);
                    materials[i].SetFloat(verticalDissolveAmount, lerpedVerticalDissolve);
                }
            }
        }

        else
        {
            float elapsedTime = 0f;
            while (elapsedTime < dissolveTime)
            {
                elapsedTime = Time.deltaTime;
                float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / dissolveTime));
                float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / dissolveTime));

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat(dissolveAmount, lerpedDissolve);
                    materials[i].SetFloat(verticalDissolveAmount, lerpedVerticalDissolve);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            isDissolving = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isDissolving = false;
        }
    }*/

}
