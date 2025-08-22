using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunSavingTypingBox : MonoBehaviour
{
    public TextMeshProUGUI typingBox;
    public TextMeshProUGUI saveAsBox;
    UIManager uiman;
    public Button confirmBtn;
    int saveToSlot; bool typing;
    string slotName;
    List<string> allowedChars;
    string currentCharacter;
    private void Start()
    {
        uiman = GetComponentInParent<UIManager>();
        allowedChars = new List<string> {
        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
        "1","2","3","4","5","6","7","8","9","0"," "};
        confirmBtn.interactable = false;
    }
    public void PromptNameing(int slot)
    {
        if (typing) { return; }
        saveToSlot = slot;
        saveAsBox.text = "SAVE TO SLOT " + slot + " AS...";
        typingBox.text = "Start typing...";
        if (typing) { StopCoroutine(StartTyping()); typing = false; }
        uiman = GetComponentInParent<UIManager>();
        allowedChars = new List<string> {
        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
        "1","2","3","4","5","6","7","8","9","0"," "};
        confirmBtn.interactable = false;
        StartCoroutine(StartTyping());
    }
    private void Update()
    {
        currentCharacter = Input.inputString;
    }
    IEnumerator StartTyping()
    {
        if (!typing) 
        {
            confirmBtn.interactable = false;
            slotName = "";
            typing = true;

            while (typing)
            {
                if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKey(KeyCode.Backspace)) && slotName.Length > 0)
                {
                    slotName = slotName.Substring(0, slotName.Length - 1);
                }
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.Return))
                {
                    typing = false;
                    break;
                }
                else if (Input.anyKeyDown || Input.anyKey)
                {
                    string charToBeAdded = currentCharacter;
                    if (allowedChars.Contains(charToBeAdded))
                    {
                        slotName = slotName + charToBeAdded;
                    }
                }
                typingBox.text = slotName;
                confirmBtn.interactable = slotName.Length > 1;
                yield return new WaitForEndOfFrame();
            }

            confirmBtn.interactable = true;
            yield return null; 
        }
        yield return null;
    }
    public void ConfirmSave()
    {
        StopCoroutine(StartTyping());
        typing = false;
        uiman.SaveRun(saveToSlot);
        uiman.UpdateSaveSlots();
    }
    public void Cancel()
    {
        if (typing) { StopCoroutine(StartTyping()); typing = false; }
        gameObject.SetActive(false);
    }
}
