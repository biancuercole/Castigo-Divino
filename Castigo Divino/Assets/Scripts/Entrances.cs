using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrances : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private Collider2D portalCollider;
    private ManagerData managerData;
    private TransicionEscena transicion;
    private GameMaster gm;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        transicion = FindObjectOfType<TransicionEscena>();
        managerData = FindObjectOfType<ManagerData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalCollider = GetComponent<Collider2D>();
        gm = FindObjectOfType<GameMaster>();
        audioManager = FindObjectOfType<AudioManager>();

        if (gameObject.CompareTag("Level1"))
        {
            spriteRenderer.sprite = openSprite;
            portalCollider.enabled = true;
        } else if (gameObject.CompareTag("Level2"))
        {
            spriteRenderer.sprite = closedSprite;
            portalCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (managerData.level1Finished)
        {
           if (gameObject.CompareTag("Level2"))
            {
                spriteRenderer.sprite = openSprite;
                portalCollider.enabled = true;
            } 
        }
    }

    public void ChangeScene()
    {
       
            if (gameObject.CompareTag("Level1"))
            {
            audioManager.ChangeBackgroundMusic(audioManager.gameMusic);
            transicion.SiguienteNivel("GameScene");
                Debug.Log("GameScene");
            }
            else if (gameObject.CompareTag("Level2"))
            {
                transicion.SiguienteNivel("EnemyLevel");
                Debug.Log("EnemyLevel");
            }     
    }
 /*   private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if(gameObject.CompareTag("Level1"))
            {
                transicion.SiguienteNivel("GameScene");
            } else if (gameObject.CompareTag("Level2"))
            {
                transicion.SiguienteNivel("EnemyLevel");
            }
        }
    }*/
}
