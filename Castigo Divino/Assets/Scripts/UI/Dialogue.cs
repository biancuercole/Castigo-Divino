using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEditor.Rendering.PostProcessing;
public class Dialogue : MonoBehaviour
{
    private bool isPlayerInRange;
    public bool didDialogueStart;
    private int lineIndex;
    private float typingTime = 0.05f;
    //[SerializeField] private GameObject dialogueStart;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private GameObject godesSprite;

    [SerializeField] private Trigger trigger;
    [SerializeField] private Rotation rotation;
    public UnityEvent OnBegin, OnDone;
    private bool isTyping = false;

    // verificar si se está escribiendo
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
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                nextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
                isTyping = false;
            }
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
        return lineIndex >= dialogueLines.Length;  
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