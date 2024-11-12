using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
//using UnityEditor.Rendering.PostProcessing;
public class Dialogue : MonoBehaviour
{
    private bool isPlayerInRange;
    public bool didDialogueStart;
    public int lineIndex;
    private float typingTime = 0.05f;
    //[SerializeField] private GameObject dialogueStart;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] public TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] public string[] dialogueLines;
    [SerializeField] private GameObject godesSprite;

    [SerializeField] private Trigger trigger;
    [SerializeField] private Rotation rotation;
    public UnityEvent OnBegin, OnDone;
    private bool isTyping = false;
    // verificar si se est� escribiendo
    public bool IsTyping
    {
        get { return isTyping; }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!didDialogueStart)
            {
                Debug.Log("Iniciando diálogo...");
                StartDialogue();
            }
            else if (isTyping)
            {
                Debug.Log("Completando línea actual...");
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
                isTyping = false;
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                if (lineIndex == dialogueLines.Length - 1)
                {
                    Debug.Log("Última línea del diálogo alcanzada. Finalizando diálogo y abriendo tienda.");
                    EndDialogue();
                }
                else
                {
                    Debug.Log("Avanzando a la siguiente línea del diálogo.");
                    nextDialogueLine();
                }
            }
        }
        if (gameObject.CompareTag("Shop") && lineIndex == 5)
        {
            trigger.CloseShop();
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Diálogo finalizado. Estado de didDialogueStart: " + didDialogueStart);
        didDialogueStart = false;
        dialoguePanel.SetActive(false);
        lineIndex = dialogueLines.Length;
        ToolTipManager.instance.HideToolTip();
        OnDone?.Invoke();
        if (gameObject.CompareTag("Shop"))
        {
            trigger.ToggleShop();
        }
    }
    private void StartDialogue()
    {
        OnBegin?.Invoke();
        trigger.targetObject.SetActive(false);
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        if (this.gameObject.CompareTag("altarVida"))
        {
            godesSprite.SetActive(true);
        }
        else
        {
            godesSprite.SetActive(false);
        }
        //dialogueStart.SetActive(false);
        lineIndex = 0;
        StartCoroutine(showLine());
    }

    public void nextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(showLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            //  dialogueStart.SetActive(true);
            trigger.dialogueStartTrigger = true;
            if (this.gameObject.CompareTag("Shop"))
                trigger.ToggleShop();
            ToolTipManager.instance.HideToolTip();
            OnDone?.Invoke();
        }
    }

    private IEnumerator showLine()
    {
        isTyping = true;
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
        isTyping = false;
    }

    public bool IsDialogueFinished()
    {
        bool isFinished = lineIndex == dialogueLines.Length;
        Debug.Log("¿El diálogo ha terminado? " + isFinished);
        return isFinished;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            //  dialogueStart.SetActive(true);
            rotation.canShoot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // dialogueStart.SetActive(false);
            rotation.canShoot = true;
        }
    }
}