using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppearChoco : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName = "Level2"; // Name of the scene to wait for
    
    // Start is called before the first frame update
    void Start()
    {
        // Hide chocolate initially
        gameObject.SetActive(false);
        
        // Check if Level2 is already loaded, if not wait for it
        CheckForTargetScene();
        
        // Listen for scene loading events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void CheckForTargetScene()
    {
        // Check if the target scene is currently loaded
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
        
        if (targetScene.isLoaded)
        {
            ActivateChocolate();
        }
        else
        {
            Debug.Log("AppearChoco: Waiting for " + targetSceneName + " to load...");
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is our target scene
        if (scene.name == targetSceneName)
        {
            ActivateChocolate();
        }
    }
    
    void ActivateChocolate()
    {
        gameObject.SetActive(true);
        Debug.Log("AppearChoco: Chocolate activated - " + targetSceneName + " has been loaded!");
    }
    
    void OnDestroy()
    {
        // Remove the scene loaded listener when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
