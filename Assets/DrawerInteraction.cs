using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteraction : MonoBehaviour
{
    [Header("Drawer Settings")]
    public float maxOpenDistance = 1f; // Maximum distance the drawer can be pulled out
    public float dragSpeed = 1f; // Speed of the dragging interaction
    public Vector3 dragDirection = Vector3.forward; // Direction the drawer opens (local space)
    
    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    
    private Vector3 originalPosition;
    private bool isDragging = false;
    private Camera playerCamera;
    private Vector3 lastMousePosition;
    private float currentOpenAmount = 0f; // 0 = closed, 1 = fully open
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        playerCamera = Camera.main;
        
        if (playerCamera == null)
        {
            Debug.LogWarning("DrawerInteraction: No main camera found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
        
        if (isDragging)
        {
            HandleDragging();
        }
    }
    
    void HandleMouseInput()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            CheckForDrawerClick();
        }
        
        // Check for mouse button up
        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }
    
    void CheckForDrawerClick()
    {
        if (playerCamera == null) return;
        
        // Cast ray from camera through mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        // Check if we hit this drawer
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                StartDragging();
            }
        }
    }
    
    void StartDragging()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
        Debug.Log("DrawerInteraction: Started dragging drawer");
    }
    
    void StopDragging()
    {
        if (isDragging)
        {
            isDragging = false;
            Debug.Log("DrawerInteraction: Stopped dragging drawer");
        }
    }
    
    void HandleDragging()
    {
        // Calculate mouse movement
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;
        
        // Use mouse Y movement for forward/backward dragging (negative Y = pull towards camera)
        float mouseMovement = -mouseDelta.y; // Negative because pulling down should open drawer
        
        // Convert screen movement to drawer movement
        float dragAmount = mouseMovement * dragSpeed * 0.01f; // Scale down the movement
        
        // Update open amount
        float previousOpenAmount = currentOpenAmount;
        currentOpenAmount += dragAmount;
        currentOpenAmount = Mathf.Clamp01(currentOpenAmount);
        
        // Calculate new position
        Vector3 targetPosition = originalPosition + (dragDirection.normalized * maxOpenDistance * currentOpenAmount);
        transform.localPosition = targetPosition;
        
        // Debug log to see what's happening
        Debug.Log($"DrawerInteraction: OpenAmount = {currentOpenAmount:F2}, Position = {transform.localPosition}");
        
        // Play sounds when crossing thresholds
        if (previousOpenAmount <= 0f && currentOpenAmount > 0f)
        {
            PlayOpenSound();
        }
        else if (previousOpenAmount > 0f && currentOpenAmount <= 0f)
        {
            PlayCloseSound();
        }
        
        lastMousePosition = currentMousePosition;
    }
    
    void PlayOpenSound()
    {
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
    
    void PlayCloseSound()
    {
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
    
    // Public method to get drawer open percentage (0-1)
    public float GetOpenAmount()
    {
        return currentOpenAmount;
    }
    
    // Public method to set drawer position programmatically
    public void SetOpenAmount(float amount)
    {
        currentOpenAmount = Mathf.Clamp01(amount);
        Vector3 targetPosition = originalPosition + (dragDirection.normalized * maxOpenDistance * currentOpenAmount);
        transform.localPosition = targetPosition;
    }
    
    // Public method to close the drawer completely
    public void CloseDrawer()
    {
        SetOpenAmount(0f);
    }
    
    // Public method to open the drawer completely
    public void OpenDrawer()
    {
        SetOpenAmount(1f);
    }
}
