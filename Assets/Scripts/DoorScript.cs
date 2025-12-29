using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public Animator animator;
    private bool isOpen = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen; // Flip the state

        if (isOpen)
        {
            // If opening, trigger the parameter named "open"
            animator.SetTrigger("open"); 
        }
        else
        {
            // If closing, trigger the parameter named "close"
            animator.SetTrigger("close");
        }
    }
}