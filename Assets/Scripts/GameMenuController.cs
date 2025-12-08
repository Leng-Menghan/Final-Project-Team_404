using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    // Restart the current scene
    public void RestartGame()
    {
        Time.timeScale = 1f; // Make sure game is running
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume normal time
        // Optionally hide your pause menu panel here
    }

    // Exit the game
    public void ExitGame()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
