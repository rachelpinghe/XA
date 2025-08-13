using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkWater : MonoBehaviour
{
    [Header("References")]
    public GameObject stairObject; // Drag the Stair GameObject here in the Inspector
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        // Check if the player clicked on the water
        if (gameObject.name == "Water")
        {
            // Find all water objects with "Water" tag in all scenes
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");
            
            foreach (GameObject water in waterObjects)
            {
                // Check if this water object is not the one we clicked (in different scene/layer)
                if (water != gameObject)
                {
                    water.SetActive(false);
                    Debug.Log("Water in 2D scene set inactive!");
                }
            }
            AchievementTestInput.a11 = true;

            

            GameObject stair = stairObject; // Use the public reference first
            
            // If Level2 scene is loaded, try finding the gameobject with the name "Stair"
            if (SceneManager.GetSceneByName("Level2").isLoaded)
            {
                Debug.Log("Level2 scene is loaded, trying to find Stair GameObject...");
                GameObject foundStair = GameObject.Find("Stair");
                if (foundStair != null)
                {
                    stair = foundStair;
                    Debug.Log("Found Stair GameObject in Level2 scene!");
                }
            }
            
            if (stair != null)
            {
                stair.SetActive(true);
                Debug.Log("Stair set active!");
            }
            else
            {
                Debug.LogWarning("Stair object reference is not assigned in DrinkWater script!");
            }
            
            // Also activate stair in 2D scene via ActiveStair script
            ActiveStair.ActivateStair();
            
            Debug.Log("Water clicked in 3D scene!");
        }
    }
}
