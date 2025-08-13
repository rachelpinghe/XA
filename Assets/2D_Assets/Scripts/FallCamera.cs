using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallCamera : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallSpeed = 5f; // Speed at which camera falls
    public float fallDistance = 10f; // How far down the camera should fall
    public float fall_x = 2f; // How far back the camera should move in x direction
    public bool isFalling = false; // Whether camera is currently falling
    
    [Header("Camera to Control")]
    public string cameraName = "Main Camera"; // Name of camera to control in 3D scene
    public string targetSceneName = "SampleScene"; // Name of the 3D scene
    
    private Vector3 originalPosition; // Store original camera position
    private Vector3 targetPosition; // Target position after falling
    private Quaternion targetRotation; // Target rotation after falling
    private Quaternion originalRotation; // Store original camera rotation
    private Camera targetCamera; // Reference to the camera to control
    private bool fallCompleted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find the target camera in the specified scene
        FindTargetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling && targetCamera != null)
        {
            // Move camera downward
            targetCamera.transform.position = Vector3.MoveTowards(
                targetCamera.transform.position, 
                targetPosition, 
                fallSpeed * Time.deltaTime
            );
            
            // Smoothly rotate camera to target rotation
            targetCamera.transform.rotation = Quaternion.Slerp(
                targetCamera.transform.rotation,
                targetRotation,
                fallSpeed * Time.deltaTime
            );
            
            // Check if fall is complete
            if (Vector3.Distance(targetCamera.transform.position, targetPosition) < 0.1f)
            {
                targetCamera.transform.position = targetPosition;
                targetCamera.transform.rotation = targetRotation;
                isFalling = false;
                fallCompleted = true;
                Debug.Log("FallCamera: Camera fall completed");
            }
        }
    }
    
    void FindTargetCamera()
    {
        // Find all cameras in all loaded scenes
        Camera[] allCameras = FindObjectsOfType<Camera>();
        
        foreach (Camera cam in allCameras)
        {
            // Check if this camera has the right name and is in the target scene
            if (cam.gameObject.name == cameraName && cam.gameObject.scene.name == targetSceneName)
            {
                targetCamera = cam;
                originalPosition = cam.transform.position;
                originalRotation = cam.transform.rotation;
                Debug.Log($"FallCamera: Found target camera '{cameraName}' in scene '{targetSceneName}'");
                break;
            }
        }
        
        if (targetCamera == null)
        {
            Debug.LogWarning($"FallCamera: Could not find camera '{cameraName}' in scene '{targetSceneName}'");
        }
    }
    
    // Public method to trigger the camera fall (call this from hole trigger)
    public void StartCameraFall()
    {
        if (targetCamera != null && !isFalling && !fallCompleted && SceneManager.GetSceneByName("Level3").isLoaded)
        {
            // Calculate target position (fall down by specified distance)
            targetPosition = originalPosition + Vector3.down * fallDistance + Vector3.back * fall_x;
            targetRotation = Quaternion.Euler(-15.4f, 0f, 7.51f); // Target rotation
            isFalling = true;
            
            Debug.Log($"FallCamera: Starting camera fall from {originalPosition} to {targetPosition}");
        }
        else if (fallCompleted)
        {
            Debug.Log("FallCamera: Camera has already fallen");
        }
        else
        {
            Debug.LogWarning("FallCamera: No target camera found or already falling");
        }
    }
    
    // Public method to reset camera to original position
    public void ResetCameraPosition()
    {
        if (targetCamera != null)
        {
            targetCamera.transform.position = originalPosition;
            targetCamera.transform.rotation = originalRotation;
            isFalling = false;
            fallCompleted = false;
            Debug.Log("FallCamera: Camera position and rotation reset to original");
        }
    }
    
    // Static method to trigger fall from other scripts
    public static void TriggerFall()
    {
        FallCamera fallCamera = FindObjectOfType<FallCamera>();
        if (fallCamera != null)
        {
            fallCamera.StartCameraFall();
        }
        else
        {
            Debug.LogWarning("FallCamera: No FallCamera script found in scene");
        }
    }
}
