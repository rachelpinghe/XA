using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [Header("Restart Settings")]
    public KeyCode restartKey = KeyCode.R; // Customizable restart key
    public bool enableRestart = true; // Toggle to enable/disable restart functionality
    public string current_scene = "Level1";
    private string currentScene;
    
    [Header("Debug")]
    public bool showDebugMessages = true; // Show debug messages in console

    // Start is called before the first frame update
    void Start()
    {
        if (showDebugMessages)
        {
            Debug.Log($"Restart script initialized. Press [{restartKey}] to restart the current level.");
        }
        currentScene = current_scene; // Initialize current scene
    }

    // Update is called once per frame
    void Update()
    {
        // Check if restart is enabled and the restart key is pressed
        if (enableRestart && Input.GetKeyDown(restartKey))
        {
            RestartCurrentLevel();
        }
    }

    void RestartCurrentLevel()
    {
        // Get the currently active scene name
        string sceneName = currentScene;

        if (showDebugMessages)
        {
            Debug.Log($"Restarting level: {sceneName}");
        }

        // Reset time scale in case it was paused
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(sceneName); // Unload the previous instance of the scene

        // Reload the current scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    
    // Public method to restart level (can be called from other scripts)
    public void RestartLevel()
    {
        RestartCurrentLevel();
    }
    
    // Public method to change the restart key at runtime
    public void SetRestartKey(KeyCode newKey)
    {
        restartKey = newKey;
        if (showDebugMessages)
        {
            Debug.Log($"Restart key changed to: {newKey}");
        }
    }
}
