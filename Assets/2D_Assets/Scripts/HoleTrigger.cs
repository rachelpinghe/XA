using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [Header("Hole Settings")]
    public bool isEntranceTrigger = true; // true = entrance, false = exit
    
    private SideScrollingCamera cameraScript;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private bool originalActiveState;
    private bool hasBeenTriggered = false;
    
    void Awake()
    {
        // Store original state for revival
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        originalActiveState = gameObject.activeSelf;
    }
    
    void Start()
    {
        // Find the camera script in the scene
        cameraScript = FindObjectOfType<SideScrollingCamera>();
        
        if (cameraScript == null)
        {
            Debug.LogWarning("HoleTrigger: No SideScrollingCamera found in scene!");
        }
        
        // Ensure this object has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("HoleTrigger: Collider should be set as trigger!");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if player entered the trigger
        if (other.CompareTag("Player") && cameraScript != null)
        {
            hasBeenTriggered = true;
            
            if (isEntranceTrigger)
            {
                // Player entered hole - enable vertical following
                cameraScript.EnterHole();
                Debug.Log("Player entered hole - Camera now follows vertically");
                
                // Also trigger the 3D camera fall
                FallCamera.TriggerFall();
                Debug.Log("Triggering 3D camera fall");
            }
            else
            {
                // Player exited hole - disable vertical following
                cameraScript.ExitHole();
                Debug.Log("Player exited hole - Camera returns to horizontal-only");
            }
        }
    }
    
    void OnDestroy()
    {
        // Cleanup when destroyed
        Debug.Log("HoleTrigger destroyed");
    }
    
    // Revival method
    public void ReviveHoleTrigger()
    {
        // Reset position and properties to original state
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
        gameObject.SetActive(originalActiveState);
        
        // Reset trigger state
        hasBeenTriggered = false;
        
        Debug.Log($"HoleTrigger revived at position: {originalPosition}");
    }
}
