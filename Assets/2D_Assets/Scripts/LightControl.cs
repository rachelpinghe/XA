using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    [Header("Light Control Settings")]
    public Color dimmedColor = Color.black; // Background color when lamp is off (black)
    public Color normalColor = Color.blue; // Background color when lamp is on (blue)
    public float transitionSpeed = 2.0f; // Speed of color transition
    
    [Header("Scene References")]
    public string lampLightTag = "LampLight"; // Tag of the lamp object in 3D scene
    public string threeDSceneName = "SampleScene"; // Name of the 3D scene
    
    // Private variables
    private Camera cam;
    public GameObject lampLight;
    private Color targetColor;
    private Color currentColor;
    
    void Start()
    {
        // Get the camera component
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("LightControl: No Camera component found on this GameObject!");
            return;
        }
        
        // Find the lamp light object in the 3D scene
        FindLampLight();
        
        // Start with black background (dimmed view)
        currentColor = dimmedColor;
        targetColor = dimmedColor;
        Debug.Log($"LightControl: Initializing with dimmed color {currentColor}");
        UpdateCameraBrightness();
    }

    void Update()
    {
        // Check if we need to find the lamp light (in case scene wasn't loaded yet)
        if (lampLight == null)
        {
            FindLampLight();
            Debug.Log("LampLight is null, trying to find it...");
        }

        // Check lamp status and update target color
        if (lampLight != null)
        {
            targetColor = lampLight.activeInHierarchy ? normalColor : dimmedColor;
            // Debug.Log("lamp light is not null");
        }
        else
        {
            // If lamp not found, stay dimmed (black)
            targetColor = dimmedColor;
        }
        
        // Smoothly transition to target color
        if (Vector4.Distance(currentColor, targetColor) > 0.01f)
        {
            currentColor = Color.Lerp(currentColor, targetColor, transitionSpeed * Time.deltaTime);
            UpdateCameraBrightness();
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
            Debug.Log($"LightControl: Successfully found lamp light object '{lampLight.name}' with tag '{lampLightTag}' in scene '{lampLight.scene.name}', current state: {lampLight.activeInHierarchy}");
        }
    }
    
    void UpdateCameraBrightness()
    {
        if (cam != null)
        {
            // Set camera background color directly
            cam.backgroundColor = currentColor;
            Debug.Log($"LightControl: Camera background color set to {currentColor}");
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
