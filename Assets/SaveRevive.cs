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
    
    // Static method to revive player at last checkpoint
    public static void RevivePlayer(GameObject player)
    {
        if (hasCheckpoint && player != null)
        {
            player.transform.position = lastCheckpointPosition;
            Debug.Log($"Player revived at checkpoint '{lastCheckpointID}' - Position: {lastCheckpointPosition}");
        }
        else
        {
            Debug.LogWarning("No checkpoint available for revival!");
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
        
        // Clear PlayerPrefs
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("LastCheckpointID");
        
        Debug.Log("All checkpoints reset");
    }
    
    void Update()
    {
        
    }
}
