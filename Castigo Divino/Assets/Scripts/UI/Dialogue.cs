using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
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

    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(!didDialogueStart)
            {
                StartDialogue();
            } else if (dialogueText.text == dialogueLines[lineIndex])
            {
                nextDialogueLine();
            }else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        OnBegin?.Invoke();
        trigger.targetObject.SetActive(false);
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        if(this.gameObject.CompareTag("altarVida"))
        {
            godesSprite.SetActive(true);
        } else {
            godesSprite.SetActive(false);
        }
        //dialogueStart.SetActive(false);
        lineIndex = 0;
        StartCoroutine(showLine());
    }

    private void nextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < dialogueLines.Length)
        {
            StartCoroutine(showLine());
        } else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
          //  dialogueStart.SetActive(true);
            trigger.dialogueStartTrigger = true;
            if(this.gameObject.CompareTag("Shop"))
            trigger.ToggleShop();
            ToolTipManager.instance.HideToolTip();
            OnDone?.Invoke();
        }
    }

    private IEnumerator showLine()
    {
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            //  dialogueStart.SetActive(true);
            rotation.canShoot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
           // dialogueStart.SetActive(false);
           rotation.canShoot = true;
        }
    }
}
