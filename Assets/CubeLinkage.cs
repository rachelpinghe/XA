using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeLinkage : MonoBehaviour
{
    [Header("Linkage Settings")]
    public float movementScale = 1f; // Scale factor for movement synchronization
    public string cubeTag = "Player"; // Tag of the cube to sync with in 2D scene
    public string level1SceneName = "Level1"; // Name of the Level1 scene
    
    private GameObject targetCube; // Reference to the cube in 2D scene
    private Vector3 lastCubePosition; // Last recorded position of the cube
    private bool isLevel1Loaded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Check if Level1 is already loaded
        CheckLevel1Status();
        
        // Subscribe to scene loaded events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        // Only sync movement when Level1 is loaded
        if (isLevel1Loaded && targetCube != null)
        {
            SyncMovementWithCube();
        }
    }
    
    void CheckLevel1Status()
    {
        // Check if Level1 scene is currently loaded
        Scene level1Scene = SceneManager.GetSceneByName(level1SceneName);
        if (level1Scene.isLoaded)
        {
            isLevel1Loaded = true;
            FindTargetCube();
        }
        else
        {
            isLevel1Loaded = false;
            targetCube = null;
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == level1SceneName)
        {
            isLevel1Loaded = true;
            Debug.Log("CubeLinkage: Level1 loaded, starting cube synchronization");
            
            // Wait a frame for objects to be properly initialized
            StartCoroutine(FindTargetCubeDelayed());
        }
    }
    
    void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == level1SceneName)
        {
            isLevel1Loaded = false;
            targetCube = null;
            Debug.Log("CubeLinkage: Level1 unloaded, stopping cube synchronization");
        }
    }
    
    IEnumerator FindTargetCubeDelayed()
    {
        yield return null; // Wait one frame
        FindTargetCube();
    }
    
    void FindTargetCube()
    {
        if (!isLevel1Loaded) return;
        
        // Find all GameObjects with the specified tag across all loaded scenes
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(cubeTag);
        
        foreach (GameObject cube in cubes)
        {
            // Check if this cube is in the Level1 scene
            if (cube.scene.name == level1SceneName)
            {
                targetCube = cube;
                lastCubePosition = targetCube.transform.position;
                Debug.Log("CubeLinkage: Found target cube '" + targetCube.name + "' in Level1");
                break;
            }
        }
        
        if (targetCube == null)
        {
            Debug.LogWarning("CubeLinkage: Could not find cube with tag '" + cubeTag + "' in Level1 scene");
        }
    }
    
    void SyncMovementWithCube()
    {
        // Calculate the change in cube position
        Vector3 currentCubePosition = targetCube.transform.position;
        Vector3 cubeMovement = currentCubePosition - lastCubePosition;
        
        // Only sync if there was actual movement
        if (cubeMovement.magnitude > 0.001f) // Small threshold to avoid floating point errors
        {
            // Apply movement to player with scale factor (only X and Y components)
            Vector3 playerMovement = new Vector3(
                cubeMovement.x * movementScale,
                cubeMovement.y * movementScale,
                0 // Don't affect Z movement of player
            );
            
            // Move the player
            transform.position += playerMovement;
            
            Debug.Log($"CubeLinkage: Cube moved {cubeMovement}, Player moved {playerMovement}");
        }
        
        // Update last cube position
        lastCubePosition = currentCubePosition;
    }
}
