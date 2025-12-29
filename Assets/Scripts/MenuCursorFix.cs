using UnityEngine;

public class MenuCursorFix : MonoBehaviour
{
    void Start()
    {
        // Unlock the cursor so you can move it
        Cursor.lockState = CursorLockMode.None;
        // Make the cursor visible
        Cursor.visible = true;
    }
}