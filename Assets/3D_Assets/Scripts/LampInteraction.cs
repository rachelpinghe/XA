using UnityEngine;

public class LampInteraction : MonoBehaviour
{
    [Header("Lamp Settings")]
    public string lampLightObjectName = "LampLight"; // Name of the lamp object to control
    public bool startActive = false; // Whether the lamp starts on or off
    public AudioClip clickSound; // Optional sound when clicking the table

    public GameObject lampLight; // Reference to the actual lamp light object
    
    [Header("Visual Feedback")]
    public Material highlightMaterial; // Optional material to show when hovering over table
    public float highlightIntensity = 1.2f; // How much brighter when hovering
    
    // Private variables
    private AudioSource audioSource;
    private Renderer tableRenderer; // Renderer of the table (this object)
    private Material originalMaterial;
    private bool isHighlighted = false;
    
    void Start()
    {
        // Find the lamp light object to control
        FindLampLight();
        
        // Set initial lamp state
        if (lampLight != null)
        {
            lampLight.SetActive(startActive);
        }
        
        // Get components for this table object
        audioSource = GetComponent<AudioSource>();
        tableRenderer = GetComponent<Renderer>();
        
        // Store original material of the table
        if (tableRenderer != null)
        {
            originalMaterial = tableRenderer.material;
        }
        
        // Ensure the table has a collider for mouse detection
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("LampInteraction: No Collider found on table! Adding BoxCollider for mouse detection.");
            gameObject.AddComponent<BoxCollider>();
        }
        
        Debug.Log($"LampInteraction: Table interaction initialized. Lamp active: {(lampLight != null ? lampLight.activeInHierarchy : false)}");
    }
    
    void OnMouseDown()
    {
        // Toggle lamp on/off when clicked
        ToggleLamp();
    }
    
    void OnMouseEnter()
    {
        // Debug.Log("LampInteraction: Mouse entered lamp area");
        // Highlight when mouse hovers over table
        if (!isHighlighted)
        {
            HighlightTable(true);
        }
    }
    
    void OnMouseExit()
    {
        // Remove highlight when mouse leaves table
        if (isHighlighted)
        {
            HighlightTable(false);
        }
    }
    
    void FindLampLight()
    {
        // Try to find the lamp light object by name
        Debug.Log($"LampInteraction: Looking for lamp light object '{lampLightObjectName}'");
        lampLight = GameObject.Find(lampLightObjectName);
        
        if (lampLight == null)
        {
            Debug.LogWarning($"LampInteraction: Could not find GameObject named '{lampLightObjectName}' in scene.");
        }
        else
        {
            Debug.Log($"LampInteraction: Found lamp light object '{lampLightObjectName}'");
        }
    }
    
    public void ToggleLamp()
    {
        Debug.Log("LampInteraction: Toggling lamp state");

        Debug.Log("Lamp Light found");
        bool newState = !lampLight.activeInHierarchy;
        lampLight.SetActive(newState);
        
        Debug.Log($"LampInteraction: Lamp turned {(newState ? "ON" : "OFF")} by clicking table");
        
        // Play click sound if available
        PlayClickSound();
    }
    
    public void SetLampActive(bool active)
    {
        if (lampLight != null)
        {
            lampLight.SetActive(active);
            Debug.Log($"LampInteraction: Lamp set to {(active ? "ON" : "OFF")}");
        }
    }
    
    void HighlightTable(bool highlight)
    {
        if (tableRenderer == null) return;
        
        isHighlighted = highlight;
        
        if (highlight)
        {
            // Apply highlight effect to the table
            if (highlightMaterial != null)
            {
                tableRenderer.material = highlightMaterial;
            }
            else
            {
                // Brighten the existing material
                Color originalColor = tableRenderer.material.color;
                tableRenderer.material.color = originalColor * highlightIntensity;
            }
        }
        else
        {
            // Restore original material
            tableRenderer.material = originalMaterial;
        }
    }
    
    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
    
    // Public method to check if lamp is active
    public bool IsLampActive()
    {
        return lampLight != null && lampLight.activeInHierarchy;
    }
}
