using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public bool isForwardExit; // Check for END trigger, Uncheck for START

    private void OnTriggerEnter(Collider other)
    {   Debug.Log("Something hit the trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            // Find the identity of THIS room
            RoomIdentity room = FindObjectOfType<RoomIdentity>();
            
            if (isForwardExit)
            {
                room.CheckForward();
            }
            else
            {
                room.CheckBackward();
            }
        }
    }
}