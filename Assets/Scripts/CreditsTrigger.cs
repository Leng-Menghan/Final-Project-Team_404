using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsTrigger : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject creditsPanel; // Drag your Overlay Panel here

    [Header("Timing")]
    public float creditsDuration = 15f; // How long to wait before Menu loads

    // This runs automatically when he walks into the "Finish" cube
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Hit the finish line! Rolling credits...");
            StartCoroutine(PlayEndingSequence());
        }
    }

    IEnumerator PlayEndingSequence()
    {
        // 1. Turn on the UI (The Animation on the panel will start auto-playing)
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }

        // 2. Wait for the text to scroll up
        yield return new WaitForSeconds(creditsDuration);

        // 3. Load the Menu
        SceneManager.LoadScene("MenuScene");
    }
}