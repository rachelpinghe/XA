using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeskGameController : MonoBehaviour
{
    [Header("Camera Setup")]
    public Camera deskCamera; // Main camera positioned at desk level
    public float normalFOV = 60f;
    public float zoomedFOV = 30f;
    public float zoomSpeed = 2f;
    
    [Header("Laptop Screen Setup")]
    public RenderTexture laptopScreenTexture;
    public int textureWidth = 1920;
    public int textureHeight = 1080;
    public Material laptopScreenMaterial;
    public string materialTextureProperty = "_MainTex";
    
    [Header("2D Game Settings")]
    public string gameSceneName = "1-1";
    private Camera gameCamera;
    
    [Header("UI")]
    public Canvas instructionCanvas;
    public TextMeshProUGUI instructionText;
    public KeyCode playGameKey = KeyCode.F;
    public KeyCode exitGameKey = KeyCode.Escape;
    public KeyCode toggleModeKey = KeyCode.Tab;
    public KeyCode zoomInKey = KeyCode.E;
    
    [Header("Game Controls")]
    // public KeyCode zoomInKey = KeyCode.E;
    // public KeyCode zoomOutKey = KeyCode.Q;
    public float gameZoomSpeed = 2f;
    public float minGameZoom = 0.5f;
    public float maxGameZoom = 3f;
    
    [Header("Room Navigation")]
    public float mouseSensitivity = 100f;
    
    // State management
    private bool isGameLoaded = false;
    private bool isZoomedIn = false;
    private float currentGameZoom = 1f;
    private string defaultInstructionText = "Press [F] to play game\nPress [Tab] to toggle mouse look";

    private bool isEscaped = false;
    
    // Mouse look
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool mouseLookEnabled = false;
    
    void Start()
    {
        SetupRenderTexture();
        SetupUI();
    }
    
    void Update()
    {
        // Debug: Add frame counter to see if Update is still running
        if (Time.frameCount % 120 == 0) // Log every 2 seconds
        {
            // Debug.Log($"DeskGameController Update running - Frame: {Time.frameCount}, Game Loaded: {isGameLoaded}");
        }
        
        HandleInput();
        HandleMouseLook();
        UpdateUI();
    }
    
    void SetupRenderTexture()
    {
        // Create render texture if not assigned
        if (laptopScreenTexture == null)
        {
            laptopScreenTexture = new RenderTexture(textureWidth, textureHeight, 16);
            laptopScreenTexture.Create();
        }
        
        // Apply render texture to laptop screen material
        if (laptopScreenMaterial != null)
        {
            laptopScreenMaterial.SetTexture(materialTextureProperty, laptopScreenTexture);
        }
    }
    
    void SetupUI()
    {
        if (instructionText != null)
        {
            instructionText.text = defaultInstructionText;
        }
    }
    
    void HandleInput()
    {
        // Debug: Test if Input system is working at all
        if (isGameLoaded && Time.frameCount % 60 == 0)
        {
            // Debug.Log($"Input Status - anyKey: {Input.anyKey}, Tab: {Input.GetKey(KeyCode.Tab)}, F: {Input.GetKey(KeyCode.F)}");
        }
        
        // Toggle mouse look
        if (Input.GetKeyDown(toggleModeKey))
        {
            // Debug.Log("Tab key pressed - toggling mouse look");
            mouseLookEnabled = !mouseLookEnabled;
        }
        
        // Game loading controls
        if (Input.GetKeyDown(playGameKey))
        {
            if (!isGameLoaded)
            {
                LoadGame();
                isGameLoaded = true;
            }
        }

        // Exit game
        if (Input.GetKeyDown(exitGameKey) && isGameLoaded)
        {
            ZoomCameraOut();
            isEscaped = true;
        }

        if (Input.GetKeyDown(zoomInKey) && isGameLoaded)
        {
            if (!isZoomedIn)
            {
                ZoomCameraToLaptop();
            }
        }

        // Camera zoom when game is loaded
        if (isGameLoaded && !isEscaped)
        {
            ZoomCameraToLaptop();

            // Game zoom controls
            if (gameCamera != null)
            {
                if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals))
                {
                    currentGameZoom = Mathf.Clamp(currentGameZoom + gameZoomSpeed * Time.deltaTime, minGameZoom, maxGameZoom);
                    gameCamera.orthographicSize = 5f / currentGameZoom;
                }

                if (Input.GetKey(KeyCode.Minus))
                {
                    currentGameZoom = Mathf.Clamp(currentGameZoom - gameZoomSpeed * Time.deltaTime, minGameZoom, maxGameZoom);
                    gameCamera.orthographicSize = 5f / currentGameZoom;
                }
            }
        }
    }
    
    void HandleMouseLook()
    {
        // if (mouseLookEnabled && !isGameLoaded)
        if (mouseLookEnabled)
        {
            // Mouse look when not playing game
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            deskCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
        else
        {
            // Free cursor for UI interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void UpdateUI()
    {
        if (instructionText == null) return;
        
        if (!isGameLoaded)
        {
            string mouseLookStatus = mouseLookEnabled ? "ON" : "OFF";
            instructionText.text = $"{defaultInstructionText}\nMouse Look: {mouseLookStatus}";
        }
        else
        {
            // string zoomInstruction = isZoomedIn ? $"Press [{zoomOutKey}] to zoom out" : $"Press [{zoomInKey}] to zoom in";
            instructionText.text = $"Game loaded! Press [{exitGameKey}] to zoom out\nPress [{zoomInKey}] to zoom in\nPress [{toggleModeKey}] to toggle mouse look";
        }
    }
    
    void ZoomCameraToLaptop()
    {
        if (isZoomedIn) return;
        
        StartCoroutine(SmoothZoom(normalFOV, zoomedFOV));
        isZoomedIn = true;
    }
    
    void ZoomCameraOut()
    {
        if (!isZoomedIn) return;
        
        StartCoroutine(SmoothZoom(zoomedFOV, normalFOV));
        isZoomedIn = false;
    }
    
    System.Collections.IEnumerator SmoothZoom(float fromFOV, float toFOV)
    {
        float elapsed = 0f;
        float duration = 1f / zoomSpeed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            deskCamera.fieldOfView = Mathf.Lerp(fromFOV, toFOV, t);
            yield return null;
        }
        
        deskCamera.fieldOfView = toFOV;
    }
    
    void LoadGame()
    {
        // Debug.Log("LoadGame called - about to load Mario scene");
        try
        {
            SceneManager.LoadScene(gameSceneName, LoadSceneMode.Additive);
            isGameLoaded = true;
            Debug.Log("Mario scene loaded successfully, starting camera setup");
            StartCoroutine(SetupGameCamera());
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene {gameSceneName}: {e.Message}");
            isGameLoaded = false;
        }
    }
    
    void UnloadGame()
    {
        if (isGameLoaded)
        {
            // Clean up camera reference
            if (gameCamera != null)
            {
                gameCamera = null;
            }
            
            // Zoom camera back out if zoomed in
            if (isZoomedIn)
            {
                ZoomCameraOut();
            }
            
            SceneManager.UnloadSceneAsync(gameSceneName);
            isGameLoaded = false;
            currentGameZoom = 1f;
        }
    }
    
    System.Collections.IEnumerator SetupGameCamera()
    {
        Debug.Log("SetupGameCamera started");
        yield return new WaitForEndOfFrame();
        
        Scene gameScene = SceneManager.GetSceneByName(gameSceneName);
        Debug.Log($"Game scene loaded status: {gameScene.isLoaded}");
        
        if (gameScene.isLoaded)
        {
            GameObject[] rootObjects = gameScene.GetRootGameObjects();
            Debug.Log($"Found {rootObjects.Length} root objects in Mario scene");
            
            foreach (GameObject obj in rootObjects)
            {
                Debug.Log($"Root object: {obj.name}");
                Camera cam = obj.GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    gameCamera = cam;
                    Debug.Log($"Found Mario camera: {cam.name}");
                    
                    // Disable ALL MonoBehaviour scripts on the camera that might cause movement/flickering
                    MonoBehaviour[] allScripts = cam.GetComponents<MonoBehaviour>();
                    Debug.Log($"Found {allScripts.Length} scripts on camera");
                    foreach (MonoBehaviour script in allScripts)
                    {
                        Debug.Log($"Camera has script: {script.GetType().Name}");
                        // Don't disable Camera component itself, but disable movement scripts
                        if (script.GetType().Name == "SideScrollingCamera" || 
                            script.GetType().Name.Contains("Camera") ||
                            script.GetType().Name.Contains("Follow") ||
                            script.GetType().Name.Contains("Track") ||
                            script.GetType().Name.Contains("Movement") ||
                            script.GetType().Name.Contains("Controller"))
                        {
                            script.enabled = false;
                            Debug.Log($"Disabled script: {script.GetType().Name} to prevent flickering");
                        }
                    }
                    
                    // Also check the parent object for movement scripts
                    if (cam.transform.parent != null)
                    {
                        MonoBehaviour[] parentScripts = cam.transform.parent.GetComponents<MonoBehaviour>();
                        Debug.Log($"Found {parentScripts.Length} scripts on camera parent");
                        foreach (MonoBehaviour script in parentScripts)
                        {
                            Debug.Log($"Camera parent has script: {script.GetType().Name}");
                            if (script.GetType().Name.Contains("Camera") ||
                                script.GetType().Name.Contains("Follow") ||
                                script.GetType().Name.Contains("Track") ||
                                script.GetType().Name.Contains("Movement") ||
                                script.GetType().Name.Contains("Controller"))
                            {
                                script.enabled = false;
                                Debug.Log($"Disabled parent script: {script.GetType().Name}");
                            }
                        }
                    }
                    
                    // Disable audio listener to avoid conflicts
                    AudioListener audioListener = cam.GetComponent<AudioListener>();
                    if (audioListener != null)
                    {
                        audioListener.enabled = false;
                        Debug.Log("Disabled Mario camera AudioListener");
                    }
                    
                    // Set initial game settings
                    currentGameZoom = 1f;
                    gameCamera.orthographicSize = 5f;
                    
                    // Assign render texture to camera
                    gameCamera.targetTexture = laptopScreenTexture;
                    Debug.Log("Assigned render texture to Mario camera");
                    
                    // Configure Mario camera to only see 2D layers
                    int marioCullingMask = 0;
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Player"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Enemies"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Environment"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Collectibles"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Background"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Effects"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_UI"));
                    marioCullingMask |= (1 << LayerMask.NameToLayer("2D_Projectiles"));
                    
                    // Fallback: if layers don't exist yet, include common 2D layers
                    if (marioCullingMask == 0)
                    {
                        // Include default layers but exclude known 3D layers
                        marioCullingMask = -1; // Everything
                        marioCullingMask &= ~(1 << LayerMask.NameToLayer("3D_Environment"));
                        marioCullingMask &= ~(1 << LayerMask.NameToLayer("3D_Props"));
                        Debug.Log("Using fallback culling mask - consider setting up 2D layers");
                    }
                    
                    gameCamera.cullingMask = marioCullingMask;
                    Debug.Log($"Mario camera culling mask set to: {marioCullingMask}");
                    
                    // Make sure camera is positioned to see Mario at start and LOCK IT
                    gameCamera.transform.position = new Vector3(0f, 6.5f, -10f);
                    
                    // Freeze the camera transform to prevent any movement
                    if (gameCamera.transform.parent == null)
                    {
                        // Create a parent object to lock the camera position
                        GameObject cameraParent = new GameObject("FixedCameraParent");
                        cameraParent.transform.position = new Vector3(0f, 6.5f, -10f);
                        gameCamera.transform.SetParent(cameraParent.transform);
                        gameCamera.transform.localPosition = Vector3.zero;
                        Debug.Log("Locked camera position to prevent movement");
                    }
                    
                    Debug.Log($"Camera configured - Position: {gameCamera.transform.position}, Culling Mask: {gameCamera.cullingMask}");
                    
                    break;
                }
            }
            
            if (gameCamera == null)
            {
                Debug.LogWarning("No camera found in the loaded game scene!");
            }
            
            // Test input immediately after setup
            Debug.Log($"Post-setup input test - anyKey: {Input.anyKey}");
        }
        else
        {
            Debug.LogError("Game scene failed to load!");
        }
    }
}
