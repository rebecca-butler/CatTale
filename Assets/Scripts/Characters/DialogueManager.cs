using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;
    public event Action OnFinishedDialogue;

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; private set; }

    // Singleton to store instance of class
    public static DialogueManager Instance { get; private set; }

    private void Awake() {
       Instance = this; 
    }

    public IEnumerator ShowDialogue(Dialogue dialogue, Action onFinished=null) {
        // Wait until next frame to prevent skipping first line of dialogue
        // because HandleUpdate() is called in the same frame and z is still pressed
        yield return new WaitForEndOfFrame();

        // Invoke event that game controller subscribes to
        OnShowDialogue?.Invoke();

        // Show the dialogue box
        IsShowing = true;
        this.dialogue = dialogue;
        OnFinishedDialogue = onFinished;

        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public void HandleUpdate() {
        // If player presses z key and dialogue isn't currently being typed, show next line of dialogue
        if (Input.GetKeyDown(KeyCode.Z) && isTyping == false) {
            ++currentLine;
            if (currentLine < dialogue.Lines.Count) {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            } else {
                // Reset, close dialogue box and emit event
                currentLine = 0;
                IsShowing = false;
                dialogueBox.SetActive(false);
                OnFinishedDialogue?.Invoke();
                OnCloseDialogue?.Invoke();
            }
        }
    }

    /* Smoothly type input dialogue on screen */
    public IEnumerator TypeDialogue(string line) {
        isTyping = true;

        dialogueText.text = "";
        foreach (var letter in line.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond); // wait for (1/lettersPerSecond) after typing each letter
        }

        isTyping = false;
    }
}
