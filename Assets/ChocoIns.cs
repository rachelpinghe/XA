using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoIns : MonoBehaviour
{
    [Header("Popup Settings")]
    public GameObject popupMessage; // The popup message GameObject to activate
    public float popupDuration = 3f; // How long to show the popup (in seconds)
    
    private SpriteRenderer spriteRenderer;
    private bool popupTriggered = false; // Prevent multiple triggers
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogWarning("ChocoIns: No SpriteRenderer found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if sprite exists and hasn't triggered popup yet
        if (spriteRenderer != null && !popupTriggered)
        {
            // Check if Y position is below -2
            if (transform.position.y < -2f)
            {
                TriggerPopup();
            }
        }
    }
    
    void TriggerPopup()
    {
        popupTriggered = true;
        
        if (popupMessage != null)
        {
            popupMessage.SetActive(true);
            StartCoroutine(HidePopupAfterDelay());
            Debug.Log("ChocoIns: Popup activated - sprite Y position below -2");
        }
        else
        {
            Debug.LogWarning("ChocoIns: Popup message GameObject is not assigned!");
        }
    }
    
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        
        if (popupMessage != null)
        {
            popupMessage.SetActive(false);
            Debug.Log("ChocoIns: Popup deactivated after " + popupDuration + " seconds");
        }
    }
    
    // Optional: Reset the trigger (useful for testing or if sprite moves back up)
    public void ResetPopupTrigger()
    {
        popupTriggered = false;
    }
}
