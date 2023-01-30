using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;

    // Singleton to store instance of class
    public static DialogueManager Instance { get; private set; }

    private void Awake() {
       Instance = this; 
    }

    public void ShowDialogue(Dialogue dialogue) {
        // Show the dialogue box
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    /* Slowly type input dialogue on screen */
    public IEnumerator TypeDialogue(string line) {
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray()) {
            dialogueText.text += letter;
            // After adding each letter to processed text, wait
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
