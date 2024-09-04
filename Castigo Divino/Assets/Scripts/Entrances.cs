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
    // Start is called before the first frame update
    void Start()
    {
        managerData = FindObjectOfType<ManagerData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalCollider = GetComponent<Collider2D>();
        if(gameObject.CompareTag("Level1"))
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
}
