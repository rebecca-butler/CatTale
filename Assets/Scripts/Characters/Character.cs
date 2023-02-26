using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }

    CharacterAnimator animator;

    private void Awake() {
        animator = GetComponent<CharacterAnimator>();
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
        if (!IsWalkable(targetPos)) {
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

    /* Check if target position is a walkable tile */
    private bool IsWalkable(Vector3 targetPos) {
        // Check if a circle centered at the target position with radius of 0.3 
        // overlaps with a solid or interactable object - if so, tile is not walkable 
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null) {
            return false;
        }
        return true;
    }

    public CharacterAnimator Animator {
        get => animator;
    }
}
