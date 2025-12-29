using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioSource doorSpeaker;
    public AudioClip openClip;
    
    private bool hasPlayed = false; // To make sure it doesn't play twice

    void Start()
    {
        // Auto-find the speaker if you forgot to drag it in
        if (doorSpeaker == null) 
            doorSpeaker = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            // Play the sound once
            doorSpeaker.PlayOneShot(openClip);
            hasPlayed = true; 
            
            
        }
    }
}