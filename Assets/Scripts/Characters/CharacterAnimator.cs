using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    // Sprites
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    // Animators
    SpriteAnimator walkDownAnimator;
    SpriteAnimator walkUpAnimator;
    SpriteAnimator walkLeftAnimator;
    SpriteAnimator walkRightAnimator;

    SpriteAnimator currAnimator;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;

    private void Start() {
        // Initialize sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize animators for each direction
        walkDownAnimator = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimator = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkLeftAnimator = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnimator = new SpriteAnimator(walkRightSprites, spriteRenderer);

        // Start with walk down animator
        currAnimator = walkDownAnimator;
    }

    private void Update() {
        var prevAnimator = currAnimator;

        // Update current animator based on movement params
        if (MoveX == 1) {
            currAnimator = walkRightAnimator;
        }
        else if (MoveX == -1) {
            currAnimator = walkLeftAnimator;
        }
        else if (MoveY == 1) {
            currAnimator = walkUpAnimator;
        }
        else if (MoveY == -1) {
            currAnimator = walkDownAnimator;
        }

        // If the animator or movement status changed, call the current animator's start function
        if (currAnimator != prevAnimator || IsMoving != wasPreviouslyMoving) {
            currAnimator.Start();
        }

        // If character is moving, handle update for current animator
        if (IsMoving) {
            currAnimator.HandleUpdate();
        } else {
            spriteRenderer.sprite = currAnimator.frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }

}
