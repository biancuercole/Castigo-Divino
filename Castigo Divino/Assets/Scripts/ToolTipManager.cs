using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;
    public TextMeshProUGUI textComponent;
    public float showTimer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition; 
        showTimer -= Time.deltaTime;
        if (showTimer <= 0)
        {
            HideToolTip();
        }
    }

    public void SetAndShowToolTip(string messeage, float showTimerMax = 1.5f)
    {
        gameObject.SetActive(true);
        textComponent.text = messeage;
        showTimer = showTimerMax;
        Update();
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
}
