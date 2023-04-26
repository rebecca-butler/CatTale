using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public event Action OnEncountered;

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
                StartCoroutine(character.Move(input, CheckForEncounters));
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

    /* Check if the player should encounter an enemy */
    private void CheckForEncounters() {
        // Check if player position overlaps with the grass layer
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null) {
            // If player is on grass, encounter enemy 10% of the time
            if (UnityEngine.Random.Range(1, 101)  <= 10) {
                character.Animator.IsMoving = false;
                OnEncountered();
            }
        }
    }
}
