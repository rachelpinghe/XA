using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWater : MonoBehaviour
{
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
            
            Debug.Log("Water clicked in 3D scene!");
        }
    }
}
