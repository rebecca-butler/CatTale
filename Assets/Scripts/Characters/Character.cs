using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }
    public float OffsetY { get; private set; } = 0.3f;

    CharacterAnimator animator;

    private void Awake() {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    /* Snap position to center of tile */
    public void SetPositionAndSnapToTile(Vector2 pos) {
        // Snap x coordinate to center
        pos.x = Mathf.Floor(pos.x) + 0.5f;

        // Snap y coordinate to slightly above center for perspective
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    /* Coroutine to move character towards target */
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null) {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        // update target position based on movement input
        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        // exit coroutine if target position is not walkable
        if (!IsPathClear(targetPos)) {
            yield break;
        }

        IsMoving = true;

        // While character hasn't reached target, continue moving it towards target
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        // Invoke the given event to check if an encounter should occur
        OnMoveOver?.Invoke();
    }

    public void HandleUpdate() {
        animator.IsMoving = IsMoving;
    }

    /* Check if all tiles on path are walkable */
    private bool IsPathClear(Vector3 targetPos) {
        // Get vector difference between target pos and current pos
        var diff = targetPos - transform.position;
        var diff_dir = diff.normalized;
        var diff_mag = diff.magnitude;

        // Cast a box from current pos to target pos. Return true if there's a collider in the box
        // Start position is current pos plus 1 because the box should start at the next tile
        // Box magnitude is difference minus 1 because it started one tile over
        if (Physics2D.BoxCast(transform.position + diff_dir, new Vector2(0.2f, 0.2f), 0f, diff_dir, diff_mag - 1,
                              GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer)) {
            return false;
        }
        return true;
    }

    /* Check if target position is a walkable tile */
    private bool IsWalkable(Vector3 targetPos) {
        // Check if a circle centered at the target position with radius of 0.3 
        // overlaps with a solid or interactable object - if so, tile is not walkable 
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null) {
            return false;
        }
        return true;
    }

    /* Make character look towards target */
    public void LookTowards(Vector3 targetPos) {
        // Get x and y difference as a number of tiles
        var x_diff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var y_diff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (x_diff == 0 || y_diff == 0) {
            animator.MoveX = Mathf.Clamp(x_diff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(y_diff, -1f, 1f);
        } else {
            Debug.LogError("[Character.LookTowards] character cannot look diagonally!");
        }
    }

    public CharacterAnimator Animator {
        get => animator;
    }
}
