using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject;
    public float height = 1f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f;
    
    [Header("Hole Following Settings")]
    public bool followPlayerInHole = false; // Whether camera should follow player vertically
    public float verticalFollowSpeed = 3f; // Speed of vertical following when in hole
    
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
            // Normal side-scrolling behavior
            Vector3 cameraPosition = transform.position;
            
            // Always follow horizontally (only move forward, never back)
            cameraPosition.x = Mathf.Max(cameraPosition.x, trackedObject.position.x);
            
            // Follow vertically only when in hole
            if (followPlayerInHole)
            {
                // Smoothly follow player's Y position when in hole
                float targetY = trackedObject.position.y;
                cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetY, verticalFollowSpeed * Time.deltaTime);
            }
            
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
    
    // Method to enable/disable hole following
    public void SetHoleFollowing(bool enabled)
    {
        followPlayerInHole = enabled;
        Debug.Log($"Camera hole following: {(enabled ? "ENABLED" : "DISABLED")}");
    }
    
    // Method to enter hole (enable vertical following)
    public void EnterHole()
    {
        SetHoleFollowing(true);
    }
    
    // Method to exit hole (disable vertical following, return to fixed height)
    public void ExitHole()
    {
        SetHoleFollowing(false);
        
        // Optionally return to normal height
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = height;
        transform.position = cameraPosition;
    }
    
    // Public method to check if camera is currently relocating
    public bool IsRelocating()
    {
        return isRelocating;
    }

}
