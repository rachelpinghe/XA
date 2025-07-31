using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    [Header("Light Control Settings")]
    public float dimmedIntensity = 0.3f; // How dim the camera should be when lamp is off
    public float normalIntensity = 1.0f; // Normal brightness when lamp is on
    public float transitionSpeed = 2.0f; // Speed of brightness transition
    
    [Header("Scene References")]
    public string lampLightObjectName = "LampLight"; // Name of the lamp object in 3D scene
    public string threeDSceneName = "SampleScene"; // Name of the 3D scene
    
    // Private variables
    private Camera cam;
    private GameObject lampLight;
    private float targetIntensity;
    private float currentIntensity;
    
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
        
        // Start with dimmed view
        currentIntensity = dimmedIntensity;
        targetIntensity = dimmedIntensity;
        UpdateCameraBrightness();
    }

    void Update()
    {
        // Check if we need to find the lamp light (in case scene wasn't loaded yet)
        if (lampLight == null)
        {
            FindLampLight();
        }
        
        // Check lamp status and update target intensity
        if (lampLight != null)
        {
            targetIntensity = lampLight.activeInHierarchy ? normalIntensity : dimmedIntensity;
        }
        else
        {
            // If lamp not found, stay dimmed
            targetIntensity = dimmedIntensity;
        }
        
        // Smoothly transition to target intensity
        if (Mathf.Abs(currentIntensity - targetIntensity) > 0.01f)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, transitionSpeed * Time.deltaTime);
            UpdateCameraBrightness();
        }
    }
    
    void FindLampLight()
    {
        // Try to find the lamp light object by name
        lampLight = GameObject.Find(lampLightObjectName);
        
        if (lampLight == null)
        {
            Debug.LogWarning($"LightControl: Could not find GameObject named '{lampLightObjectName}' in scene.");
        }
        else
        {
            Debug.Log($"LightControl: Found lamp light object '{lampLightObjectName}'");
        }
    }
    
    void UpdateCameraBrightness()
    {
        if (cam != null)
        {
            // Method 1: Adjust camera's background color alpha/brightness
            Color bgColor = cam.backgroundColor;
            bgColor = Color.black * currentIntensity;
            cam.backgroundColor = bgColor;
            
            // Method 2: You could also use a post-processing effect or overlay
            // For now, we'll use background color dimming
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
