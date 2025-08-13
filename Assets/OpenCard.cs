using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCard : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject uiPanel; // Drag your UI panel here in the inspector
    // public GameObject alternativeObject; // Optional alternative object to check for clicks
    
    private Camera playerCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        
        if (playerCamera == null)
        {
            Debug.LogWarning("OpenCard: No main camera found!");
        }
        
        // Make sure the panel starts inactive
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseClick();
    }
    
    void HandleMouseClick()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            CheckForCardClick();
        }
    }
    
    void CheckForCardClick()
    {
        if (playerCamera == null) return;
        
        // Cast ray from camera through mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        Debug.Log("OpenCard: Mouse clicked, casting ray...");
        
        // Check if we hit this card
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"OpenCard: Hit object: {hit.collider.gameObject.name}");
            
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("OpenCard: Hit matches this card!");
                OpenPanel();
            }
            else
            {
                Debug.Log("OpenCard: Hit different object, not this card");
            }
        }
        else
        {
            Debug.Log("OpenCard: No raycast hit detected");
        }
    }
    
    void OpenPanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            Debug.Log("OpenCard: UI Panel activated");
        }
        else
        {
            Debug.LogWarning("OpenCard: No UI Panel assigned!");
        }
    }
    
    // Public method to close the panel (can be called from other scripts or UI buttons)
    public void ClosePanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            Debug.Log("OpenCard: UI Panel deactivated");
        }
    }
}
