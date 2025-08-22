using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SendFeedback : MonoBehaviour
{
    public TextMeshProUGUI typingBox;
    UIManager uiman;
    public Button sendBtn;
    bool typing;
    string currentCharacter;
    private void Start()
    {
        uiman = GetComponentInParent<UIManager>();
        sendBtn.interactable = false;
    }
    public void PromptTyping()
    {
        StopAllCoroutines();
        typing = false;
        typingBox.text = "";
        uiman = GetComponentInParent<UIManager>();
        sendBtn.interactable = false;
        StartCoroutine(StartTyping());
    }
    private void Update()
    {
        currentCharacter = Input.inputString;
    }
    IEnumerator StartTyping()
    {
        sendBtn.interactable = false;
        typingBox.text = "";
        typing = true;

        while (typing)
        {
            if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKey(KeyCode.Backspace)) && typingBox.text.Length > 0)
            {
                typingBox.text = typingBox.text.Substring(0, typingBox.text.Length - 1);
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.Return))
            {
                typing = false;
                break;
            }
            else if (Input.anyKeyDown || Input.anyKey)
            {
                string charToBeAdded = currentCharacter;
                typingBox.text = typingBox.text + charToBeAdded;
            }
            sendBtn.interactable = typingBox.text.Length > 1;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    public void SendEmail()
    {
        StopAllCoroutines();
        typing = false;
        uiman.healthManager.gdm.instance.AddFeedbackEmailToQue(typingBox.text);
        typingBox.text = "";
    }
}
