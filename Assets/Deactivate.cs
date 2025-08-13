using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deactivate : MonoBehaviour
{
    [Header("Deactivation Settings")]
    public GameObject targetGameObject; // The GameObject to deactivate when button is clicked
    public bool deactivateButton = false; // Whether to also deactivate this button after clicking
    
    private Button button; // Reference to the button component
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();
        
        if (button != null)
        {
            // Add the deactivation method to the button's onClick event
            button.onClick.AddListener(DeactivateTarget);
            Debug.Log("Deactivate script initialized - button click listener added");
        }
        else
        {
            Debug.LogError("Deactivate script: No Button component found on this GameObject!");
        }
        
        // Check if target GameObject is assigned
        if (targetGameObject == null)
        {
            Debug.LogWarning("Deactivate script: No target GameObject assigned! Please assign one in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Method called when button is clicked
    public void DeactivateTarget()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false);
            Debug.Log($"Deactivated GameObject: {targetGameObject.name}");
        }
        else
        {
            Debug.LogWarning("Deactivate: Cannot deactivate - no target GameObject assigned!");
        }
        
        // Optionally deactivate this button as well
        if (deactivateButton && button != null)
        {
            button.gameObject.SetActive(false);
            Debug.Log("Deactivated button itself");
        }
    }
    
    // Public method to reactivate the target (can be called from other scripts)
    public void ReactivateTarget()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(true);
            Debug.Log($"Reactivated GameObject: {targetGameObject.name}");
        }
    }
}
