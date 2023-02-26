using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPatterns;

    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;

    private void Awake() {
        character = GetComponent<Character>();
    }

    public void Interact() {
        if (state == NPCState.Idle) {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
        }
    }

    public void Update() {
        if (DialogueManager.Instance.IsShowing) {
            return;
        }

        if (state == NPCState.Idle) {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPatterns) {
                idleTimer = 0f;
                if (movementPattern.Count > 0) {
                    StartCoroutine(Walk());
                }
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk() {
        state = NPCState.Walking;

        yield return character.Move(movementPattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}

public enum NPCState { Idle, Walking }