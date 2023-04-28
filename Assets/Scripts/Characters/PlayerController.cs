using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISavable
{
    private Vector2 input;

    private Character character;

    private void Awake() {
        character = GetComponent<Character>();
    }

    /* Update player position */
    public void HandleUpdate() {
        if (!character.IsMoving) {
            // Get user input from arrow keys
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Don't allow diagonal movement
            if (input.x != 0) input.y = 0;

            // If input is non-zero, move player
            if (input != Vector2.zero) {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        // If the user pushes Z, the player interacts with the object in front of it
        if (Input.GetKeyDown(KeyCode.Z)) {
            Interact();
        }
    }

    /* Interact with valid interactable objects */
    void Interact() {
        // Get the direction the player is facing in
        var facingDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        // Get the position of the tile to interact with
        var interactPos = transform.position + facingDirection;
        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        // If there is an interactable object in the interact position, interact with it
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null) {
            // Call the Interact function of the derived class from the Interactable base interface
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    /* Check if any triggerables were encountered */
    private void OnMoveOver() {
        // Get all colliders that overlap with the circle
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.OffsetY), 0.2f, GameLayers.i.TriggerableLayers);

        // For each collider, if they are triggerable, call the callback
        foreach(var collider in colliders) {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null) {
                Debug.Log(collider);
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

    /* Store player state on save */
    public object CaptureState() {
        float[] position = new float[] { transform.position.x, transform.position.y };
        return position;
    }

    /* Restore player state on load */
    public void RestoreState(object state) {
        var position = (float[])state;
        transform.position = new Vector3(position[0], position[1]);
    }

    public Character Character => character;
}
