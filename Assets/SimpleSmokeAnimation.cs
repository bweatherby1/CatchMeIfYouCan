using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSmokeAnimation : MonoBehaviour
{
    [Header("Assign all frames of the smoke animation")]
    public Sprite[] smokeFrames;          // Drag PixelSmoke_0 to PixelSmoke_3 here in order

    [Header("Time in seconds between frames")]
    public float frameRate = 0.1f;        // Adjust for faster/slower animation

    private SpriteRenderer sr;           // Reference to the sprite renderer
    private int currentFrame = 0;        // Which frame is showing
    private float timer = 0f;            // Time counter

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (smokeFrames.Length > 0)
        {
            sr.sprite = smokeFrames[0]; // Start with first frame
        }
    }

    void Update()
    {
        if (smokeFrames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f; // Reset the timer

            currentFrame++; // Go to next frame

            // If we hit the end, loop
            if (currentFrame >= smokeFrames.Length)
            {
                currentFrame = 0;
            }

            sr.sprite = smokeFrames[currentFrame];
        }
    }
}
