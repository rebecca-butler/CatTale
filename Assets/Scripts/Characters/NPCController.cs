using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;

    public void Interact() {
        DialogueManager.Instance.ShowDialogue(dialogue);
        Debug.Log("Interacting with NPC");
    }
}
