using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    /* Update player position */
    private void Update() {
        if (!isMoving) {
            // Get user input from arrow keys
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Don't allow diagonal movement
            if (input.x != 0) input.y = 0;

            // If input is non-zero, move player
            if (input != Vector2.zero) {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // Only move player if target position is walkable
                if (IsWalkable(targetPos)) {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

        // If the user pushes Z, the player interacts with the object in front of it
        if (Input.GetKeyDown(KeyCode.Z)) {
            Interact();
        }
    }

    /* Coroutine to move player towards target */
    IEnumerator Move(Vector3 targetPos) {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    /* Interact with valid interactable objects */
    void Interact() {
        // Get the direction the player is facing in
        var facingDirection = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        // Get the position of the tile to interact with
        var interactPos = transform.position + facingDirection;
        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        // If there is an interactable object in the interact position, interact with it
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null) {
            // Call the Interact function of the derived class from the Interactable base interface
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    /* Check if target position is a walkable tile */
    private bool IsWalkable(Vector3 targetPos) {
        // Check if a circle centered at the target position with radius of 0.3 
        // overlaps with a solid or interactable object - if so, tile is not walkable 
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null) {
            return false;
        }
        return true;
    }
}
