using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    private int enemyCount = 0;
    private int keysCollected = 0;
    private bool doorClosed = true;

    [SerializeField] private int keysNeeded;
    [SerializeField] private int enemyNeeded;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    private Collider2D doorCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();

        if (doorClosed)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else
        {
            spriteRenderer.sprite = openSprite;
        }
    }

    public void EnemyDefeated()
    {
        enemyCount++;
        CheckOpenDoor();
    }

    public void collectKey()
    {
        keysCollected++;
        CheckOpenDoor();
    }

    private void CheckOpenDoor()
    {
        if (keysCollected >= keysNeeded && enemyCount >= enemyNeeded && doorClosed)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        doorClosed = false;
        doorCollider.enabled = false;
        spriteRenderer.sprite = openSprite;
    }

    public void closeDoor()
    {
        keysCollected = 0;
        enemyCount = 0;
        doorClosed = true;
        spriteRenderer.sprite = closedSprite;
        doorCollider.enabled = true;
    }

    public void ChangeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void ResetVariables()
    {
        keysCollected = 0;
        enemyCount = 0;
        doorClosed = true;
        spriteRenderer.sprite = closedSprite;
        doorCollider.enabled = true;
    }
}
