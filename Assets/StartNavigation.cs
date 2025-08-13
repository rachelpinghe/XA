using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartNavigation : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject firstPersonCamera; // The first person camera to activate
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Movement speed
    public float mouseSensitivity = 2f; // Mouse sensitivity for looking around
    
    [Header("UI Settings")]
    public GameObject instructionUI; // UI panel for instructions
    public TextMeshProUGUI instructionText; // Text component for instructions
    public float instructionDisplayTime = 5f; // How long to show instructions
    
    [Header("Mouse Control")]
    public bool enableMouseLook = true; // Whether mouse look is enabled by default
    
    private bool isMouseLookEnabled;
    private CharacterController characterController;
    private Camera fpCamera;
    private float verticalRotation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set up first person camera
        SetupFirstPersonCamera();
        
        // Get components
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            // If no CharacterController, try to find one on the firstPersonCamera or its parent
            if (firstPersonCamera != null)
            {
                characterController = firstPersonCamera.GetComponent<CharacterController>();
                if (characterController == null && firstPersonCamera.transform.parent != null)
                {
                    characterController = firstPersonCamera.transform.parent.GetComponent<CharacterController>();
                }
            }
        }
        
        if (firstPersonCamera != null)
        {
            fpCamera = firstPersonCamera.GetComponent<Camera>();
        }
        
        // Initialize mouse look state
        isMouseLookEnabled = enableMouseLook;
        
        // Show instructions to player
        ShowInstructions();
        
        // Set cursor state based on mouse look
        UpdateCursorState();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Tab key to toggle mouse view
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMouseLook();
        }
        
        // Handle movement
        HandleMovement();
        
        // Handle mouse look
        if (isMouseLookEnabled)
        {
            HandleMouseLook();
        }
    }
    
    void SetupFirstPersonCamera()
    {
        if (firstPersonCamera != null)
        {
            firstPersonCamera.SetActive(true);
            Debug.Log("StartNavigation: First person camera activated");
        }
        else
        {
            Debug.LogWarning("StartNavigation: First person camera not assigned!");
        }
    }
    
    void HandleMovement()
    {
        if (characterController == null) return;
        
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys
        float vertical = Input.GetAxis("Vertical"); // W/S keys
        
        // Calculate movement direction relative to camera
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = transform.TransformDirection(direction);
        direction *= moveSpeed;
        
        // Apply gravity
        direction.y = -9.81f;
        
        // Move the character
        characterController.Move(direction * Time.deltaTime);
    }
    
    void HandleMouseLook()
    {
        if (fpCamera == null) return;
        
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate the camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        fpCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    
    void ShowInstructions()
    {
        if (instructionUI != null)
        {
            instructionUI.SetActive(true);
            
            if (instructionText != null)
            {
                instructionText.text = "Welcome! Use WASD to explore the room.\nPress TAB to toggle mouse view.";
            }
            
            // Hide instructions after specified time
            StartCoroutine(HideInstructionsAfterDelay());
        }
        else
        {
            // Fallback: print to console if no UI assigned
            Debug.Log("Welcome! Use WASD to explore the room. Press TAB to toggle mouse view.");
        }
    }
    
    private IEnumerator HideInstructionsAfterDelay()
    {
        yield return new WaitForSeconds(instructionDisplayTime);
        
        if (instructionUI != null)
        {
            instructionUI.SetActive(false);
        }
    }
    
    void ToggleMouseLook()
    {
        isMouseLookEnabled = !isMouseLookEnabled;
        UpdateCursorState();
        
        Debug.Log("StartNavigation: Mouse look " + (isMouseLookEnabled ? "enabled" : "disabled"));
    }
    
    void UpdateCursorState()
    {
        if (isMouseLookEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    // Public method to get mouse look state (for other scripts)
    public bool IsMouseLookEnabled()
    {
        return isMouseLookEnabled;
    }
}
