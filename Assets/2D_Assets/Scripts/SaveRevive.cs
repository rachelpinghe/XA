using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRevive : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public string checkpointID = "Checkpoint_01"; // Unique ID for this checkpoint
    public bool isActivated = false; // Whether this checkpoint has been activated
    public GameObject visualIndicator; // Optional visual feedback (light, particle, etc.)
    
    [Header("Audio Feedback")]
    public AudioClip activationSound; // Sound when checkpoint is activated
    public AudioSource audioSource; // Audio source component
    
    [Header("Player Settings")]
    public string playerTag = "Player"; // Tag of the player object
    public static float start_x = -7.95f; // Make static so RevivePlayer can access it
    public static Vector3 defaultStartingPosition = new Vector3(-7.95f, 0.604f, 0f); // Static default position

    // Static variables to track checkpoint system
    public static Vector3 lastCheckpointPosition = Vector3.zero;
    public static string lastCheckpointID = "";
    public static bool hasCheckpoint = false;
    
    void Start()
    {
        // Get audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // Check if this was the last activated checkpoint
        if (lastCheckpointID == checkpointID && hasCheckpoint)
        {
            ActivateCheckpoint(false); // Activate without sound/effects
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isActivated)
        {
            ActivateCheckpoint(true);
            SavePlayerProgress(other.transform);
        }
    }
    
    void ActivateCheckpoint(bool playEffects)
    {
        isActivated = true;
        
        // Visual feedback
        if (visualIndicator != null)
        {
            visualIndicator.SetActive(true);
        }

        // Activate the "Activated" child GameObject
        Transform activatedChild = transform.Find("Activated");
        if (activatedChild != null)
        {
            activatedChild.gameObject.SetActive(true);
            Debug.Log($"Checkpoint '{checkpointID}': Activated child object 'Activated'");
        }
        else
        {
            Debug.LogWarning($"Checkpoint '{checkpointID}': No child object named 'activated' found!");
        }
        
        // Audio feedback
        if (playEffects && audioSource != null && activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
        
        // Update static checkpoint data
        lastCheckpointPosition = transform.position;
        lastCheckpointID = checkpointID;
        hasCheckpoint = true;
        
        Debug.Log($"Checkpoint '{checkpointID}' activated at position {transform.position}");
    }

    void SavePlayerProgress(Transform player)
    {
        // Save player position
        Vector3 savePosition = player.position;

        // You can extend this to save other player data like:
        // - Health
        // - Score
        // - Inventory items
        // - Level progress

        Debug.Log($"Player progress saved at checkpoint '{checkpointID}' - Position: {savePosition}");

        // Optional: Save to PlayerPrefs for persistent storage
        PlayerPrefs.SetFloat("CheckpointX", savePosition.x);
        PlayerPrefs.SetFloat("CheckpointY", savePosition.y);
        PlayerPrefs.SetFloat("CheckpointZ", savePosition.z);
        PlayerPrefs.SetString("LastCheckpointID", checkpointID);
        PlayerPrefs.Save();
    }
    public static void RevivePlayer(GameObject player)
    {
        if (hasCheckpoint && player != null)
        {
            player.transform.position = lastCheckpointPosition;
            Debug.Log($"Player revived at checkpoint '{lastCheckpointID}' - Position: {lastCheckpointPosition}");
        }
        else if (player != null)
        {
            // Use the static default starting position
            player.transform.position = defaultStartingPosition;
            Debug.Log($"No checkpoint found, player revived at starting position: {defaultStartingPosition}");
        }
        else
        {
            Debug.LogError("Cannot revive player: player GameObject is null!");
            return;
        }
        
        // Relocate camera to player position after revival
        SideScrollingCamera camera = FindObjectOfType<SideScrollingCamera>();
        if (camera != null)
        {
            camera.RelocateToPlayer();
        }
        else
        {
            Debug.LogWarning("SaveRevive: No SideScrollingCamera found in scene!");
        }
    }
    
    // Static method to load checkpoint from PlayerPrefs
    public static void LoadCheckpointFromSave()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            string checkpointID = PlayerPrefs.GetString("LastCheckpointID", "");
            
            lastCheckpointPosition = new Vector3(x, y, z);
            lastCheckpointID = checkpointID;
            hasCheckpoint = true;
            
            Debug.Log($"Loaded checkpoint '{checkpointID}' from save - Position: {lastCheckpointPosition}");
        }
    }
    
    // Method to reset all checkpoints (useful for new game)
    public static void ResetCheckpoints()
    {
        lastCheckpointPosition = Vector3.zero;
        lastCheckpointID = "";
        hasCheckpoint = false;

        // Find all SaveRevive instances and deactivate their "Activated" child
        SaveRevive[] checkpoints = GameObject.FindObjectsOfType<SaveRevive>();
        foreach (SaveRevive checkpoint in checkpoints)
        {
            Transform activatedChild = checkpoint.transform.Find("Activated");
            if (activatedChild != null)
            {
                activatedChild.gameObject.SetActive(false);
            }
        }

        // Clear PlayerPrefs
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("LastCheckpointID");
        
        Debug.Log("All checkpoints reset");
    }
    
    // Method to reset this individual checkpoint's state
    public void ResetCheckpointState()
    {
        isActivated = false;
        
        // Deactivate the "activated" child GameObject (lowercase)
        Transform activatedChild = transform.Find("activated");
        if (activatedChild != null)
        {
            activatedChild.gameObject.SetActive(false);
            Debug.Log($"Checkpoint '{checkpointID}': Deactivated child object 'activated'");
        }
        
        // Also check for "Activated" (uppercase) for compatibility
        Transform activatedChildUpper = transform.Find("Activated");
        if (activatedChildUpper != null)
        {
            activatedChildUpper.gameObject.SetActive(false);
            Debug.Log($"Checkpoint '{checkpointID}': Deactivated child object 'Activated'");
        }
        
        // Deactivate visual indicator if exists
        if (visualIndicator != null)
        {
            visualIndicator.SetActive(false);
        }
        
        Debug.Log($"Checkpoint '{checkpointID}' state reset");
    }
    
    void Update()
    {
        
    }
}
