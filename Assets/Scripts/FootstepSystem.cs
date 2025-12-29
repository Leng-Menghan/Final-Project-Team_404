using UnityEngine;
using System.Collections.Generic; // Needed for Lists

public class FootstepSystem : MonoBehaviour
{
    [Header("Settings")]
    public float stepSpeed = 0.5f; // Time between steps
    public AudioSource footstepSource; // Drag AudioSource here
    public List<AudioClip> stepSounds; // Drag your mp3s here

    private float timer = 0f;

    void Update()
    {
        // 1. Check if player is pressing WASD
        bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);

        if (isMoving)
        {
            // 2. Count down the timer
            timer -= Time.deltaTime;

            // 3. If timer hits zero, play a sound
            if (timer <= 0f)
            {
                PlayFootstep();
                timer = stepSpeed; // Reset timer
            }
        }
        else
        {
            // Reset timer so the first step happens immediately when you start walking again
            timer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (stepSounds.Count > 0 && footstepSource != null)
        {
            // Pick a random sound from the list
            int randomIndex = Random.Range(0, stepSounds.Count);
            
            // Optional: Change pitch slightly to sound natural
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            
            // Play it
            footstepSource.PlayOneShot(stepSounds[randomIndex]);
        }
    }
}