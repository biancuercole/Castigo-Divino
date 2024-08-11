using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHealth : MonoBehaviour
{
    private SpriteRenderer machineSprite;
    private NextStage nextStage;
    private Collider2D machineCollider; 
    private LootBag lootBag; // Añadido para referencia a LootBag
    
    void Start()
    {
        machineSprite = GetComponent<SpriteRenderer>(); 
        machineCollider = GetComponent<Collider2D>();   
        nextStage = FindObjectOfType<NextStage>();
        lootBag = GetComponent<LootBag>(); // Asigna el LootBag
    }

    public void machineDamage()
    {
        machineGetDamage();
    }

    private void machineGetDamage()
    {
        if (lootBag != null) // Verifica si lootBag no es nulo
        {
            lootBag.InstantiateLoot(transform.position);
            Debug.Log("se intancio el loot");
        }
        else
        {
            Debug.Log("LootBag no encontrado en la máquina.");
        }
        
        //nextStage.MachineDefeated();
        Destroy(gameObject);
    }
}
