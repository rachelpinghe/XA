using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float minFlickerTime = 0.05f; // Minimum time between flickers
    public float maxFlickerTime = 0.3f; // Maximum time between flickers
    
    private bool isFlickering = false;
    private Renderer objectRenderer;
    private Light lightComponent;

    void Start()
    {
        // Get components for flickering
        objectRenderer = GetComponent<Renderer>();
        lightComponent = GetComponent<Light>();
        
        // Start flickering immediately when game starts
        Debug.Log("Starting lamp flicker effect on game start!");
        StartCoroutine(ContinuousFlicker());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ContinuousFlicker()
    {
        isFlickering = true;
        
        while (isFlickering)
        {
            // Random flicker duration
            float flickerDuration = Random.Range(minFlickerTime, maxFlickerTime);
            
            // Randomly turn on or off
            bool turnOn = Random.Range(0f, 1f) > 0.5f;
            
            // Apply flicker state
            SetFlickerState(turnOn);
            
            // Wait for the flicker duration
            yield return new WaitForSeconds(flickerDuration);
        }
    }
    
    void SetFlickerState(bool isOn)
    {
        // Flicker the renderer (visibility)
        if (objectRenderer != null)
        {
            objectRenderer.enabled = isOn;
        }
        
        // Flicker the light component (illumination)
        if (lightComponent != null)
        {
            lightComponent.enabled = isOn;
        }
    }
    
    // Optional: Stop flickering (call this if you want to stop the effect)
    public void StopFlickering()
    {
        isFlickering = false;
        
        // Ensure lamp is visible when stopping
        SetFlickerState(true);
        
        Debug.Log("Flicker effect stopped");
    }
}
