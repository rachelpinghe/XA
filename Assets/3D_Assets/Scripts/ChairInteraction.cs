using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChairInteraction : MonoBehaviour
{
    [Header("Chair Settings")]
    public string chairObjectName = "Chair2D"; // Name of the 2D chair sprite to sync with
    
    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private GameObject chair2D;
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only enable dragging if Level3 scene is loaded
        if (!SceneManager.GetSceneByName("Level3").isLoaded)
        {
            return;
        }
        
        // Find 2D chair if not already found
        if (chair2D == null)
        {
            chair2D = GameObject.Find(chairObjectName);
            if (chair2D != null)
            {
                Debug.Log($"ChairInteraction: Found 2D chair '{chairObjectName}'");
            }
        }
        
        HandleMouseDrag();
    }
    
    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if mouse is over this chair
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    lastMousePosition = Input.mousePosition;
                    Debug.Log("ChairInteraction: Started dragging chair");
                }
            }
        }
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            // Calculate mouse movement
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition;
            
            // Convert screen space movement to world space movement
            // Use a simple scaling factor based on camera distance and field of view
            float mouseSensitivity = 0.01f; // Adjust this value to control sensitivity
            float xMovement = mouseDelta.x * mouseSensitivity * Time.deltaTime * 60f; // Frame-rate independent movement
            
            // Move the 3D chair in X position
            transform.position += new Vector3(xMovement, 0, 0);
            
            // Move the 2D chair sprite the same amount in X position
            if (chair2D != null)
            {
                chair2D.transform.position += new Vector3(xMovement, 0, 0);
            }
            
            lastMousePosition = currentMousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                Debug.Log("ChairInteraction: Stopped dragging chair");
            }
            isDragging = false;
        }
    }
}
