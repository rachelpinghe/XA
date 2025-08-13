using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public float maxRotation = 90f; // Maximum rotation angle on Y-axis
    public float rotationSpeed = 2f; // Speed of door rotation
    public bool isOpen = false; // Current state of the door
    
    [Header("Camera Movement")]
    public Vector3 targetCameraPosition = new Vector3(0, 2, -5); // Target position for main camera
    public Vector3 targetCameraRotation = new Vector3(0, 0, 0); // Target rotation for main camera (Euler angles)
    public float cameraTransitionSpeed = 2f; // Speed of camera movement
    public bool moveCameraOnClick = true; // Whether to move camera when door is clicked
    
    [Header("Ending")]
    public GameObject endingButton; // The ending button GameObject to activate after camera movement
    
    [Header("Input")]
    public KeyCode interactionKey = KeyCode.E; // Key to interact with door
    public bool useMouseClick = true; // Whether to use mouse click interaction
    
    private Vector3 closedRotation; // Initial rotation when door is closed
    private Vector3 openRotation; // Target rotation when door is open
    private bool isRotating = false; // Prevent multiple interactions during rotation
    private bool playerInRange = false; // Whether player is in interaction range
    private Camera mainCamera; // Reference to the main camera
    private Vector3 originalCameraPosition; // Store original camera position
    private Quaternion originalCameraRotation; // Store original camera rotation
    private float originalCameraFOV; // Store original camera field of view
    
    // Start is called before the first frame update
    void Start()
    {
        // Store the initial rotation as the closed position
        closedRotation = transform.rotation.eulerAngles;
        
        // Calculate the open rotation by adding maxRotation to Y-axis
        openRotation = closedRotation + new Vector3(0, maxRotation, 0);
        
        // Find and store reference to main camera
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;
            originalCameraFOV = mainCamera.fieldOfView;
        }
        else
        {
            Debug.LogWarning("DoorInteraction: No main camera found!");
        }
        
        Debug.Log($"Door initialized - Closed: {closedRotation}, Open: {openRotation}");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnMouseDown()
    {
        // Handle mouse click interaction
        if (useMouseClick)
        {
            ToggleDoor();
            
            // Move camera to target position if enabled
            if (moveCameraOnClick && mainCamera != null)
            {
                StartCoroutine(MoveCameraToTarget());
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"Player in range - Press [{interactionKey}] to interact with door");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left interaction range");
        }
    }
    
    void ToggleDoor()
    {
        // Prevent multiple interactions during rotation
        if (isRotating) return;
        
        // Toggle door state
        isOpen = !isOpen;
        
        // Start rotation coroutine
        Vector3 targetRotation = isOpen ? openRotation : closedRotation;
        StartCoroutine(RotateDoor(targetRotation));
        
        Debug.Log($"Door {(isOpen ? "opening" : "closing")} to rotation: {targetRotation}");
    }
    
    System.Collections.IEnumerator RotateDoor(Vector3 targetRotation)
    {
        isRotating = true;
        
        Vector3 startRotation = transform.rotation.eulerAngles;
        float elapsed = 0f;
        float duration = 1f / rotationSpeed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Smooth rotation interpolation
            Vector3 currentRotation = Vector3.Lerp(startRotation, targetRotation, t);
            transform.rotation = Quaternion.Euler(currentRotation);
            
            yield return null;
        }
        
        // Ensure exact final rotation
        transform.rotation = Quaternion.Euler(targetRotation);
        
        isRotating = false;
        
        Debug.Log($"Door {(isOpen ? "opened" : "closed")} successfully");
    }
    
    System.Collections.IEnumerator MoveCameraToTarget()
    {
        if (mainCamera == null) yield break;
        
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetCameraRotation);
        float startFOV = mainCamera.fieldOfView;
        float targetFOV = 70f;
        
        float elapsed = 0f;
        float duration = 1f / cameraTransitionSpeed;
        
        Debug.Log($"Moving camera to position: {targetCameraPosition}, rotation: {targetCameraRotation}, FOV: {targetFOV}");
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Smooth position, rotation, and FOV interpolation
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetCameraPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            
            yield return null;
        }
        
        // Ensure exact final values
        mainCamera.transform.position = targetCameraPosition;
        mainCamera.transform.rotation = targetRotation;
        mainCamera.fieldOfView = targetFOV;
        
        Debug.Log("Camera movement completed");
        
        // Wait 2 seconds and then activate the ending button
        yield return new WaitForSeconds(2f);
        
        if (endingButton != null)
        {
            endingButton.SetActive(true);
            Debug.Log("Ending button activated after 2 seconds");
        }
        else
        {
            Debug.LogWarning("DoorInteraction: No ending button assigned!");
        }
    }
    
    // Public method to reset camera to original position
    public void ResetCamera()
    {
        if (mainCamera != null)
        {
            StartCoroutine(ResetCameraToOriginal());
        }
    }
    
    System.Collections.IEnumerator ResetCameraToOriginal()
    {
        if (mainCamera == null) yield break;
        
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float startFOV = mainCamera.fieldOfView;
        
        float elapsed = 0f;
        float duration = 1f / cameraTransitionSpeed;
        
        Debug.Log("Resetting camera to original position");
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Smooth position, rotation, and FOV interpolation back to original
            mainCamera.transform.position = Vector3.Lerp(startPosition, originalCameraPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, originalCameraRotation, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, originalCameraFOV, t);
            
            yield return null;
        }
        
        // Ensure exact final values
        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;
        mainCamera.fieldOfView = originalCameraFOV;
        
        Debug.Log("Camera reset completed");
    }
}
