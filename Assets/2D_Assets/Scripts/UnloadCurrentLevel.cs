using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadCurrentLevel : MonoBehaviour
{
    [Header("Level to Unload")]
    public string levelName = "Level1"; // The level to unload when PassDoor activates
    
    private static string lastLoadedLevel = "";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if PassDoor has loaded a new level by monitoring the static variable
        if (PassDoor.GetCurrentLoadedLevel() != lastLoadedLevel && !string.IsNullOrEmpty(PassDoor.GetCurrentLoadedLevel()))
        {
            // PassDoor has loaded a new level
            UnloadSpecifiedLevel();
            lastLoadedLevel = PassDoor.GetCurrentLoadedLevel();
        }
    }
    
    void UnloadSpecifiedLevel()
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            Scene sceneToUnload = SceneManager.GetSceneByName(levelName);
            if (sceneToUnload.isLoaded)
            {
                SceneManager.UnloadSceneAsync(levelName);
                Debug.Log($"UnloadCurrentLevel: Unloaded {levelName}");
            }
        }
    }
}
