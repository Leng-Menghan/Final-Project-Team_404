using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonImage : MonoBehaviour
{
    public Button button;        // The Button component
    public Sprite loadImage;     // Default image
    public Sprite silentImage;   // Image to switch to

    private bool isSilent = false;

    public void ToggleImage()
    {
        isSilent = !isSilent;  // Switch state

        // Change the button's image
        button.image.sprite = isSilent ? silentImage : loadImage;
    }
}
