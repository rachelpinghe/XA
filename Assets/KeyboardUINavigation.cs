using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyboardUINavigation : MonoBehaviour
{
    [Header("Navigation Settings")]
    public List<Button> buttons = new List<Button>(); // List of buttons to navigate through
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public float highlightScale = 1.1f;
    
    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip navigationSound;
    public AudioClip selectSound;
    
    private int currentSelectedIndex = 0;
    private bool isNavigationActive = false;
    
    // Store original button states
    private List<ColorBlock> originalColorBlocks = new List<ColorBlock>();
    private List<Vector3> originalScales = new List<Vector3>();
    
    void Start()
    {
        // Auto-find all buttons in the canvas if list is empty
        if (buttons.Count == 0)
        {
            FindAllButtons();
        }
        
        // Store original button states
        StoreOriginalButtonStates();
        
        // Start navigation when any button becomes active
        CheckForActiveButtons();
    }
    
    void Update()
    {
        // Check if any buttons are active (popup is showing)
        CheckForActiveButtons();
        
        if (isNavigationActive)
        {
            HandleKeyboardInput();
        }
    }
    
    void FindAllButtons()
    {
        // Find all Button components in children
        Button[] allButtons = GetComponentsInChildren<Button>(true);
        buttons.AddRange(allButtons);
        
        Debug.Log($"KeyboardUINavigation: Found {buttons.Count} buttons for navigation");
    }
    
    void StoreOriginalButtonStates()
    {
        originalColorBlocks.Clear();
        originalScales.Clear();
        
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                originalColorBlocks.Add(button.colors);
                originalScales.Add(button.transform.localScale);
            }
        }
    }
    
    void CheckForActiveButtons()
    {
        bool hasActiveButtons = false;
        
        foreach (Button button in buttons)
        {
            if (button != null && button.gameObject.activeInHierarchy && button.interactable)
            {
                hasActiveButtons = true;
                break;
            }
        }
        
        if (hasActiveButtons && !isNavigationActive)
        {
            StartNavigation();
        }
        else if (!hasActiveButtons && isNavigationActive)
        {
            StopNavigation();
        }
    }
    
    void StartNavigation()
    {
        isNavigationActive = true;
        currentSelectedIndex = 0;
        
        // Find first active button
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null && buttons[i].gameObject.activeInHierarchy && buttons[i].interactable)
            {
                currentSelectedIndex = i;
                break;
            }
        }
        
        UpdateButtonHighlight();
        Debug.Log("KeyboardUINavigation: Navigation started");
    }
    
    void StopNavigation()
    {
        isNavigationActive = false;
        ClearAllHighlights();
        Debug.Log("KeyboardUINavigation: Navigation stopped");
    }
    
    void HandleKeyboardInput()
    {
        // Up arrow or W key - navigate up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateUp();
            PlayNavigationSound();
        }
        // Down arrow or S key - navigate down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateDown();
            PlayNavigationSound();
        }
        // Enter or Space - select current button
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectCurrentButton();
            PlaySelectSound();
        }
        // Escape - cancel navigation
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopNavigation();
        }
    }
    
    void NavigateUp()
    {
        int startIndex = currentSelectedIndex;
        
        do
        {
            currentSelectedIndex--;
            if (currentSelectedIndex < 0)
            {
                currentSelectedIndex = buttons.Count - 1;
            }
        }
        while (!IsButtonValid(currentSelectedIndex) && currentSelectedIndex != startIndex);
        
        UpdateButtonHighlight();
    }
    
    void NavigateDown()
    {
        int startIndex = currentSelectedIndex;
        
        do
        {
            currentSelectedIndex++;
            if (currentSelectedIndex >= buttons.Count)
            {
                currentSelectedIndex = 0;
            }
        }
        while (!IsButtonValid(currentSelectedIndex) && currentSelectedIndex != startIndex);
        
        UpdateButtonHighlight();
    }
    
    bool IsButtonValid(int index)
    {
        if (index < 0 || index >= buttons.Count) return false;
        
        Button button = buttons[index];
        return button != null && button.gameObject.activeInHierarchy && button.interactable;
    }
    
    void UpdateButtonHighlight()
    {
        // Clear all highlights first
        ClearAllHighlights();
        
        // Highlight current button
        if (IsButtonValid(currentSelectedIndex))
        {
            HighlightButton(currentSelectedIndex);
            Debug.Log($"KeyboardUINavigation: Selected button {currentSelectedIndex}: {buttons[currentSelectedIndex].name}");
        }
    }
    
    void ClearAllHighlights()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null && i < originalColorBlocks.Count && i < originalScales.Count)
            {
                // Restore original colors and scale
                buttons[i].colors = originalColorBlocks[i];
                buttons[i].transform.localScale = originalScales[i];
            }
        }
    }
    
    void HighlightButton(int index)
    {
        if (!IsButtonValid(index)) return;
        
        Button button = buttons[index];
        
        // Change button colors
        ColorBlock colors = button.colors;
        colors.normalColor = highlightColor;
        colors.highlightedColor = highlightColor;
        colors.selectedColor = highlightColor;
        button.colors = colors;
        
        // Scale up the button slightly
        button.transform.localScale = originalScales[index] * highlightScale;
    }
    
    void SelectCurrentButton()
    {
        if (IsButtonValid(currentSelectedIndex))
        {
            Button selectedButton = buttons[currentSelectedIndex];
            selectedButton.onClick.Invoke();
            Debug.Log($"KeyboardUINavigation: Invoked button: {selectedButton.name}");
        }
    }
    
    void PlayNavigationSound()
    {
        if (audioSource != null && navigationSound != null)
        {
            audioSource.PlayOneShot(navigationSound);
        }
    }
    
    void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
    }
    
    // Public method to manually set buttons list
    public void SetButtons(List<Button> newButtons)
    {
        buttons = newButtons;
        StoreOriginalButtonStates();
    }
    
    // Public method to add a single button
    public void AddButton(Button button)
    {
        if (button != null && !buttons.Contains(button))
        {
            buttons.Add(button);
            originalColorBlocks.Add(button.colors);
            originalScales.Add(button.transform.localScale);
        }
    }
}
