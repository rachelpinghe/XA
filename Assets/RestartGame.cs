using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [Header("Restart Settings")]
    public string firstSceneName = "SampleScene"; // The first scene to load (main scene)
    public bool resetStaticVariables = true; // Whether to reset static variables
    public float restartDelay = 0f; // Optional delay before restarting
    
    [Header("Revival Objects")]
    public HoleTrigger holeTriggerToRevive; // Manually assign the hole trigger to revive
    
    private Button button; // Reference to the button component
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();
        
        if (button != null)
        {
            // Add the restart method to the button's onClick event
            button.onClick.AddListener(RestartWholeGame);
            Debug.Log("RestartGame script initialized - button click listener added");
        }
        else
        {
            Debug.LogError("RestartGame script: No Button component found on this GameObject!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Method called when button is clicked
    public void RestartWholeGame()
    {
        Debug.Log("Restarting whole game...");
        
        if (restartDelay > 0f)
        {
            StartCoroutine(RestartWithDelay());
        }
        else
        {
            PerformRestart();
        }
    }
    
    System.Collections.IEnumerator RestartWithDelay()
    {
        Debug.Log($"Waiting {restartDelay} seconds before restart...");
        yield return new WaitForSeconds(restartDelay);
        PerformRestart();
    }
    
    void PerformRestart()
    {
        // Reset static variables if enabled
        if (resetStaticVariables)
        {
            ResetStaticVariables();
        }
        
        // Reset time scale in case it was paused
        Time.timeScale = 1f;
        
        // Load the first scene (this will unload all other scenes)
        SceneManager.LoadScene(firstSceneName);
        
        Debug.Log($"Game restarted - loaded scene: {firstSceneName}");
    }
    
    void ResetStaticVariables()
    {
        // Reset any static variables from your game
        // Add your specific static variable resets here
        
        // Example: Reset FakeDoor ending state
        if (System.Type.GetType("FakeDoor") != null)
        {
            FakeDoor.ResetEnding();
            Debug.Log("Reset FakeDoor.Ending static variable");
        }
        
        // Revive specific HoleTrigger if assigned
        if (holeTriggerToRevive != null)
        {
            holeTriggerToRevive.ReviveHoleTrigger();
            Debug.Log("Revived assigned HoleTrigger");
        }
        else
        {
            Debug.LogWarning("RestartGame: No HoleTrigger assigned for revival");
        }
        
        // Example: Reset PassDoor current level
        if (System.Type.GetType("PassDoor") != null)
        {
            // PassDoor doesn't have a reset method, but you could add one
            Debug.Log("PassDoor static variables noted for reset");
        }
        
        Debug.Log("Static variables reset completed");
    }
    
    // Public method to restart immediately (can be called from other scripts)
    public void RestartImmediately()
    {
        restartDelay = 0f;
        RestartWholeGame();
    }
    
    // Public method to restart with custom delay
    public void RestartWithCustomDelay(float delay)
    {
        restartDelay = delay;
        RestartWholeGame();
    }
}
