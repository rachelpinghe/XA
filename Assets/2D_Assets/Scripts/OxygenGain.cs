using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGain : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 10f; // Maximum oxygen time in seconds
    public float currentOxygen;
    
    [Header("UI References")]
    public Slider oxygenBar; // Reference to the oxygen progress bar
    public Canvas oxygenCanvas; // Reference to the canvas containing the oxygen bar
    
    [Header("Water Detection")]
    public string waterAreaTag = "WaterArea"; // Tag for the water area
    
    private bool isInWater = false;
    private bool isDead = false;
    private LifeNumber lifeScript;
    
    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = maxOxygen;
        lifeScript = FindObjectOfType<LifeNumber>();
        
        // Hide oxygen bar initially
        if (oxygenCanvas != null)
        {
            oxygenCanvas.gameObject.SetActive(false);
        }
        
        // Initialize oxygen bar
        if (oxygenBar != null)
        {
            oxygenBar.maxValue = maxOxygen;
            oxygenBar.value = currentOxygen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInWater && !isDead)
        {
            // Decrease oxygen over time
            currentOxygen -= Time.deltaTime;
            
            // Update progress bar
            if (oxygenBar != null)
            {
                oxygenBar.value = currentOxygen;
            }
            
            // Check if oxygen depleted
            if (currentOxygen <= 0)
            {
                PlayerDrowned();
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterWater();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitWater();
        }
    }
    
    void EnterWater()
    {
        isInWater = true;
        
        // Show oxygen bar
        if (oxygenCanvas != null)
        {
            oxygenCanvas.gameObject.SetActive(true);
        }
        
        Debug.Log("Player entered water area - Oxygen countdown started!");
    }
    
    void ExitWater()
    {
        isInWater = false;
        
        // Reset oxygen when exiting water
        currentOxygen = maxOxygen;
        
        // Hide oxygen bar
        if (oxygenCanvas != null)
        {
            oxygenCanvas.gameObject.SetActive(false);
        }
        
        // Reset progress bar
        if (oxygenBar != null)
        {
            oxygenBar.value = currentOxygen;
        }
        
        Debug.Log("Player exited water area - Oxygen restored!");
    }
    
    void PlayerDrowned()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Player drowned - No oxygen left!");
            
            // Decrease life using LifeNumber script
            if (lifeScript != null)
            {
                lifeScript.life--;
                lifeScript.UpdateLifeDisplay();
                
                // Trigger player death/revival system
                lifeScript.PlayerDied();
            }
            else
            {
                Debug.LogWarning("OxygenGain: No LifeNumber script found!");
            }
            
            // Reset for next time
            ResetOxygenSystem();
        }
    }
    
    void ResetOxygenSystem()
    {
        isDead = false;
        currentOxygen = maxOxygen;
        isInWater = false;
        
        // Hide oxygen bar
        if (oxygenCanvas != null)
        {
            oxygenCanvas.gameObject.SetActive(false);
        }
        
        // Reset progress bar
        if (oxygenBar != null)
        {
            oxygenBar.value = currentOxygen;
        }
    }
}
