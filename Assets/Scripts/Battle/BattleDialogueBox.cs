using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogueText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] List<Text> actionTexts;
    
    public void SetDialogue(string dialogue) {
        dialogueText.text = dialogue;
    }

    // Animate typing of dialogue letters
    public IEnumerator TypeDialogue(string dialogue) {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond); // wait for (1/lettersPerSecond) after typing each letter
        }
    }

    // Show or hide the dialogue text UI
    public void EnableDialogueText(bool enabled) {
        dialogueText.enabled = enabled;
    }

    // Show or hide the action selector UI
    public void EnableActionSelector(bool enabled) {
        actionSelector.SetActive(enabled);
    }

    // Highlight text for selected action
    public void UpdateActionSelection(int selectedAction) {
        for (int i = 0; i < actionTexts.Count; ++i) {
            if (i == selectedAction) {
                actionTexts[i].color = GlobalSettings.i.HighlightedColor;
            } else {
                actionTexts[i].color = Color.black;
            }
        }
    }
}
