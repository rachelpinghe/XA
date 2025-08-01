using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject;
    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f;
    
    [Header("Camera Relocation Settings")]
    public float slideSpeed = 5f; // Speed of camera sliding back to player
    public bool isRelocating = false; // Whether camera is currently sliding back
    
    private Vector3 targetPosition; // Target position for smooth sliding

    private void LateUpdate()
    {
        if (isRelocating)
        {
            // Smooth slide to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            
            // Check if we're close enough to the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isRelocating = false;
                Debug.Log("Camera relocation complete");
            }
        }
        else
        {
            // Normal side-scrolling behavior (only move forward, never back)
            Vector3 cameraPosition = transform.position;
            cameraPosition.x = Mathf.Max(cameraPosition.x, trackedObject.position.x);
            transform.position = cameraPosition;
        }
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }
    
    // Method to relocate camera to player position (call when player revives)
    public void RelocateToPlayer()
    {
        if (trackedObject != null)
        {
            // Set target position to player's location
            targetPosition = new Vector3(trackedObject.position.x, transform.position.y, transform.position.z);
            isRelocating = true;
            
            Debug.Log($"Camera relocating to player at position: {targetPosition}");
        }
        else
        {
            Debug.LogWarning("SideScrollingCamera: No tracked object assigned for relocation!");
        }
    }
    
    // Method to instantly snap camera to player (alternative to smooth sliding)
    public void SnapToPlayer()
    {
        if (trackedObject != null)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.x = trackedObject.position.x;
            transform.position = cameraPosition;
            isRelocating = false;
            
            Debug.Log($"Camera snapped to player at position: {cameraPosition}");
        }
    }
    
    // Public method to check if camera is currently relocating
    public bool IsRelocating()
    {
        return isRelocating;
    }

}
