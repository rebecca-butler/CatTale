using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    public List<Sprite> frames { get; }
    SpriteRenderer spriteRenderer;
    float frameRate;

    int currentFrame;
    float timer;

    // Constructor with default framerate of 1/60 (60 fps)
    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate=0.16f) {
        this.frames = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    public void Start() {
        /* Initialization */
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate() {
        // Increment timer
        timer += Time.deltaTime;

        if (timer > frameRate) {
            // Increment current frame, or move back to first frame if end of list is reached
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }
}
