using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro; // Needed for the 3D Sign

public class LoopManager : MonoBehaviour
{
    public static LoopManager Instance;

    [Header("Game Settings")]
    public int targetStreak = 5; // How many rounds to win
    public int currentStreak = 0;

    [Header("Scene Names (Must Match Build Settings)")]
    public string normalScene = "Hallway_Normal";
    public string winScene = "WinScene";
    public string[] anomalyScenes; // List of your 10 weird rooms

    [Header("World Objects")]
    public TMP_Text streakSign; // Drag 3D Text here
    public string signPrefix = "Exit ";
    
    // The Working List (Prevents repeats during a run)
    private List<string> availableAnomalies;
    void Awake()
    {
        // Singleton Pattern: Makes this object Immortal
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Saves Manager + Sign
            RefillDeck();
            UpdateSign();
        }
        else
        {
            // If we reload the Normal scene, kill the duplicate manager
            Destroy(gameObject);
        }
    }
    
    void RefillDeck()
    {
        availableAnomalies = new List<string>(anomalyScenes);
        Debug.Log("Deck Refilled. Cards available: " + availableAnomalies.Count);
    }
    
    public void RegisterWin()
    {
        currentStreak++;
        UpdateSign(); // Update the visual sign
        Debug.Log("Correct! Streak: " + currentStreak);

        if (currentStreak >= targetStreak)
        {
            Debug.Log("Target Reached! YOU WIN!");
            currentStreak = 0; 
            SceneManager.LoadScene(winScene);
            Destroy(gameObject); // Stop the manager
        }
        else
        {
            LoadRandomRoom();
        }
    }

    public void RegisterLoss()
    {
        Debug.Log("Wrong Choice. Resetting.");
        currentStreak = 0;
        UpdateSign(); // Reset sign to 0
        
        // <--- CHANGE 3: Reset the deck so previous scenes can appear again
        RefillDeck();
        
        // Always go back to Normal Room on fail
        SceneManager.LoadScene(normalScene);
    }

    private void LoadRandomRoom()
    {
        // 75% Chance for Anomaly
        if (Random.value <= 0.75f) 
        {
            // <--- CHANGE 4: Safety Check
            // If we ran out of unique scenes but haven't won yet, refill the deck!
            if (availableAnomalies.Count == 0)
            {
                RefillDeck();
            }

            // Pick random card
            int index = Random.Range(0, availableAnomalies.Count);
            string sceneToLoad = availableAnomalies[index];

            // Remove it so it doesn't repeat immediately
            availableAnomalies.RemoveAt(index);

            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // 25% Chance Normal
            SceneManager.LoadScene(normalScene);
        }
    }

    public void UpdateSign()
    {
        if (streakSign != null)
        {
            streakSign.text = signPrefix + currentStreak.ToString();
        }
    }
}