using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunControl : MonoBehaviour
{
    [Header("Sun Movement Settings")]
    public Transform sunTransform; // The sun GameObject to move
    public Vector3 startPosition = new Vector3(0, -5, 0); // Sun position when lamp is off (below horizon)
    public Vector3 endPosition = new Vector3(0, 5, 0); // Sun position when lamp is on (above horizon)
    public float moveSpeed = 2.0f; // Speed of sun movement
    
    [Header("Scene References")]
    public string lampLightTag = "LampLight"; // Tag of the lamp object in 3D scene
    public string threeDSceneName = "SampleScene"; // Name of the 3D scene
    
    // Private variables
    private Camera cam;
    public GameObject lampLight;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    
    void Start()
    {
        // Check if sun transform is assigned
        if (sunTransform == null)
        {
            Debug.LogError("SunControl: No Sun Transform assigned! Please assign the sun GameObject in the inspector.");
            return;
        }
        
        // Find the lamp light object in the 3D scene
        FindLampLight();
        
        // Start with sun at starting position (below horizon)
        currentPosition = startPosition;
        targetPosition = startPosition;
        sunTransform.position = currentPosition;
        Debug.Log($"SunControl: Initializing sun at starting position {currentPosition}");
    }

    void Update()
    {
        // Check if we need to find the lamp light (in case scene wasn't loaded yet)
        if (lampLight == null)
        {
            FindLampLight();
            Debug.Log("LampLight is null, trying to find it...");
        }

        // Check lamp status and update target position
        if (lampLight != null)
        {
            targetPosition = lampLight.activeInHierarchy ? endPosition : startPosition;
            // Debug.Log("lamp light is not null");
        }
        else
        {
            // If lamp not found, stay at starting position (below horizon)
            targetPosition = startPosition;
        }
        
        // Smoothly move sun to target position
        if (Vector3.Distance(currentPosition, targetPosition) > 0.01f)
        {
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
            UpdateSunPosition();
        }
    }
    
    void FindLampLight()
    {
        // Try to find the lamp light object by tag
        Debug.Log($"LightControl: Looking for GameObject with tag '{lampLightTag}'");
        lampLight = GameObject.FindWithTag(lampLightTag);
        
        if (lampLight == null)
        {
            Debug.LogWarning($"LightControl: Could not find GameObject with tag '{lampLightTag}' in currently loaded scenes.");
            Debug.LogWarning("LightControl: Make sure you've created and assigned the 'LampLight' tag to your lamp object!");
            
            // Debug: List all GameObjects with "lamp" or "light" in their name
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            Debug.Log($"LightControl: Searching through {allObjects.Length} GameObjects for lamp/light objects...");
            
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("lamp") || obj.name.ToLower().Contains("light"))
                {
                    Debug.Log($"LightControl: Found object with 'lamp' or 'light' in name: '{obj.name}' (tag: '{obj.tag}') in scene '{obj.scene.name}'");
                }
            }
        }
        else
        {
            Debug.Log($"SunControl: Successfully found lamp light object '{lampLight.name}' with tag '{lampLightTag}' in scene '{lampLight.scene.name}', current state: {lampLight.activeInHierarchy}");
        }
    }
    
    void UpdateSunPosition()
    {
        if (sunTransform != null)
        {
            // Move the sun to the current position
            sunTransform.position = currentPosition;
            Debug.Log($"SunControl: Sun moved to position {currentPosition}");
        }
    }
    
    // Public method to manually set lamp status (useful for testing)
    public void SetLampActive(bool active)
    {
        if (lampLight != null)
        {
            lampLight.SetActive(active);
        }
    }
    
    // Public method to get current lamp status
    public bool IsLampActive()
    {
        return lampLight != null && lampLight.activeInHierarchy;
    }
}
