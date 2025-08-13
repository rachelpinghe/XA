using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PassDoor : MonoBehaviour
{
    public bool setActive = false;
    public string nextLevelName = "Level2"; // Name of the next level to load

    [Header("Level 4 Protection")]
    public GameObject nameInputUI; // UI panel for name input
    public TextMeshProUGUI promptText; // Text showing the prompt
    public TMP_InputField nameInputField; // Input field for name
    public Button submitButton; // Submit button
    public TextMeshProUGUI errorText; // Error message text
    public bool newLevelLoaded = false; // Flag to track if a new level has been loaded
    private string correctName = "stanley"; // The correct name to enter

    // Static variable to track current 2D level
    private static string currentLoadedLevel = "Level1";

    // Static method to get current loaded level for other scripts
    public static string GetCurrentLoadedLevel()
    {
        return currentLoadedLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set up UI elements
        if (nameInputUI != null)
        {
            nameInputUI.SetActive(false); // Hide UI initially
        }

        if (promptText != null)
        {
            promptText.text = "Level 4 is protected, please enter your name to continue!";
        }

        if (errorText != null)
        {
            errorText.text = "";
        }

        // Add listener to keep text margins at 0
        if (nameInputField != null)
        {
            nameInputField.onValueChanged.AddListener(CheckNameOnType);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (nextLevelName == "Level2")
        {
            if (other.CompareTag("Player"))
            {
                Transform activatedChild = transform.Find("Activated");
                if (activatedChild != null)
                {
                    setActive = true;
                    activatedChild.gameObject.SetActive(setActive); // Activate the "Activated" child GameObject

                    // Unload current 2D level and load new one
                    LoadNewLevel(nextLevelName);
                    newLevelLoaded = true; // Set flag to indicate a new level has been loaded
                    SceneManager.UnloadSceneAsync("Level1");
                }
                else
                {
                    Debug.LogWarning("PassDoor: Activated child not found!");
                }
            }
        }
        else if (nextLevelName == "Level3")
        {
            if (other.CompareTag("Player"))
            {
                Transform activatedChild = transform.Find("Activated");
                if (activatedChild != null)
                {
                    setActive = true;
                    activatedChild.gameObject.SetActive(setActive); // Activate the "Activated" child GameObject

                    // Unload current 2D level and load new one
                    LoadNewLevel(nextLevelName);
                    newLevelLoaded = true; // Set flag to indicate a new level has been loaded
                    SceneManager.UnloadSceneAsync("Level2");
                }
                else
                {
                    Debug.LogWarning("PassDoor: Activated child not found!");
                }
            }
        }
        else if (nextLevelName == "Level4")
        {
            currentLoadedLevel = "Level3";
            if (other.CompareTag("Player"))
            {
                // Show the name input UI
                ShowNameInput();
            }
        }
    }

    void ShowNameInput()
    {
        if (nameInputUI != null)
        {
            nameInputUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            // Clear previous input and error
            if (nameInputField != null)
            {
                nameInputField.text = "";

                // Set placeholder text if it exists
                if (nameInputField.placeholder != null)
                {
                    TMP_Text placeholderText = nameInputField.placeholder.GetComponent<TMP_Text>();
                    if (placeholderText != null)
                    {
                        placeholderText.text = "Enter your name here...";
                        //placeholderText.text = "Enter your name here...";
                    }
                    else
                    {
                        // Try TextMeshPro placeholder
                        TextMeshProUGUI placeholderTMP = nameInputField.placeholder.GetComponent<TextMeshProUGUI>();
                        if (placeholderTMP != null)
                        {
                            //placeholderTMP.text = "Enter your name here...";
                            placeholderTMP.GetComponent<TMP_Text>().text = "Enter your name here...";
                        }
                    }
                }

                // Focus the input field
                nameInputField.Select();
                nameInputField.ActivateInputField();
            }

            if (errorText != null)
            {
                errorText.text = "";
            }
        }
    }

    void CheckNameOnType(string enteredText)
    {
        // Check if the entered text matches the correct name
        string trimmedText = enteredText.Trim();
        
        if (trimmedText.Equals(correctName, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("PassDoor: Correct name entered automatically, loading Level 4!");
            // Correct name entered - automatically load Level 4
            HideNameInput();
            LoadLevel4();
        }
        else
        {
            // Clear any previous error message when typing
            if (errorText != null)
            {
                errorText.text = "";
            }
        }
    }

    void HideNameInput()
    {
        if (nameInputUI != null)
        {
            nameInputUI.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    void LoadLevel4()
    {
        Transform activatedChild = transform.Find("Activated");
        if (activatedChild != null)
        {
            setActive = true;
            activatedChild.gameObject.SetActive(setActive);
        }
        Debug.Log(currentLoadedLevel);

        // Use LoadNewLevel which handles scene switching properly
        LoadNewLevel("Level4");
        newLevelLoaded = true; // Set flag to indicate a new level has been loaded
        Debug.Log("Level 4 loaded successfully!");
    }

    void LoadNewLevel(string levelName)
    {
        // Store the previous level name before updating
        string previousLevel = currentLoadedLevel;
        
        // Update current loaded level tracker first
        currentLoadedLevel = levelName;
        
        // Load new level additively first
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        
        // Only unload previous level if it's different and not empty
        if (!string.IsNullOrEmpty(previousLevel) && previousLevel != levelName)
        {
            // Use a coroutine to delay unloading to ensure new scene is fully loaded
            StartCoroutine(UnloadPreviousLevel(previousLevel));
        }
    }
    
    private IEnumerator UnloadPreviousLevel(string levelToUnload)
    {
        // Wait one frame to ensure new scene is loaded
        yield return null;
        
        // Check if the scene exists before trying to unload
        Scene sceneToUnload = SceneManager.GetSceneByName(levelToUnload);
        if (sceneToUnload.isLoaded)
        {
            SceneManager.UnloadSceneAsync(levelToUnload);
            Debug.Log($"PassDoor: Unloaded previous level: {levelToUnload}");
        }
    }
}