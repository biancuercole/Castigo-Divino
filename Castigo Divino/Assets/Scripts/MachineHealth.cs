using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHealth : MonoBehaviour
{
    private SpriteRenderer machineSprite;
    private NextStage nextStage;
    private Collider2D machineCollider; 
    private LootBag lootBag; // Añadido para referencia a LootBag
    public GameObject damageParticle;
    public GameObject explosionPaticle;
    private AudioManager audioManager;
    private int life;

    void Start()
    {
        life = 4; 
        machineSprite = GetComponent<SpriteRenderer>(); 
        machineCollider = GetComponent<Collider2D>();   
        nextStage = FindObjectOfType<NextStage>();
        lootBag = GetComponent<LootBag>(); // Asigna el LootBag
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void machineDamage()
    {
        life --;
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        Debug.Log("Vida maquina " + life);
        if (life == 0)
        {
        machineGetDamage();
        }
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
        Destroy(gameObject);
        CameraMovement.Instance.MoveCamera(5, 5, 0.5f);
        Instantiate(explosionPaticle, transform.position, Quaternion.identity);
        audioManager.playSound(audioManager.machineDeath);
        //nextStage.MachineDefeated();
    }
}
