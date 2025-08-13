using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWhenFall : MonoBehaviour
{
    [Header("Activation Settings")]
    public GameObject objectToActivate; // GameObject to activate when falling starts
    public float activationDuration = 7f; // How long to keep active (in seconds)
    
    [Header("Fall Camera Reference")]
    public MonoBehaviour fallCameraScript; // Reference to the Fall Camera script
    
    private bool wasFalling = false; // Track previous falling state
    private bool isActivated = false; // Prevent multiple activations
    
    // Start is called before the first frame update
    void Start()
    {
        // If no Fall Camera script is assigned, try to find it on this GameObject
        if (fallCameraScript == null)
        {
            // Try to find a script with "Fall" in the name
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Contains("Fall"))
                {
                    fallCameraScript = script;
                    Debug.Log("ActivateWhenFall: Found Fall Camera script: " + script.GetType().Name);
                    break;
                }
            }
            
            if (fallCameraScript == null)
            {
                Debug.LogWarning("ActivateWhenFall: No Fall Camera script found! Please assign it manually.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fallCameraScript != null)
        {
            // Use reflection to get the IsFalling property/field
            bool isFalling = GetIsFallingValue();
            
            // Check if falling state changed from false to true
            if (isFalling && !wasFalling && !isActivated)
            {
                ActivateObject();
            }
            
            wasFalling = isFalling;
        }
    }
    
    private bool GetIsFallingValue()
    {
        // Try to get IsFalling as a property first
        var property = fallCameraScript.GetType().GetProperty("IsFalling");
        if (property != null)
        {
            return (bool)property.GetValue(fallCameraScript);
        }
        
        // Try to get IsFalling as a field
        var field = fallCameraScript.GetType().GetField("IsFalling");
        if (field != null)
        {
            return (bool)field.GetValue(fallCameraScript);
        }
        
        // Try lowercase versions
        property = fallCameraScript.GetType().GetProperty("isFalling");
        if (property != null)
        {
            return (bool)property.GetValue(fallCameraScript);
        }
        
        field = fallCameraScript.GetType().GetField("isFalling");
        if (field != null)
        {
            return (bool)field.GetValue(fallCameraScript);
        }
        
        Debug.LogWarning("ActivateWhenFall: Could not find IsFalling property or field in " + fallCameraScript.GetType().Name);
        return false;
    }
    
    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            isActivated = true;
            StartCoroutine(DeactivateAfterDelay());
            Debug.Log("ActivateWhenFall: Activated " + objectToActivate.name + " for " + activationDuration + " seconds");
        }
        else
        {
            Debug.LogWarning("ActivateWhenFall: Object to activate is not assigned!");
        }
    }
    
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDuration);
        
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
            Debug.Log("ActivateWhenFall: Deactivated " + objectToActivate.name + " after " + activationDuration + " seconds");
        }
        
        // Reset activation flag after a brief delay to allow for new triggers
        yield return new WaitForSeconds(0.5f);
        isActivated = false;
    }
}
