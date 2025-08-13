using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("FollowCamera: No main camera found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Sync sprite XY position to main camera, keep original Z
        if (mainCamera != null)
        {
            Vector3 newPosition = new Vector3(
                mainCamera.transform.position.x,
                mainCamera.transform.position.y,
                transform.position.z
            );
            transform.position = newPosition;
        }
    }
}
