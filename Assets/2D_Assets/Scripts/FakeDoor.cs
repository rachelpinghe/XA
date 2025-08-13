using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDoor : MonoBehaviour
{
    [Header("Fake Door Settings")]
    public bool destroyOnTrigger = true; // Whether to destroy this fake door after triggering
    public string playerTag = "Player"; // Tag to check for triggering
    public GameObject visualIndicator; // Optional visual feedback (light, particle, etc.)
    
    // Static boolean that other scripts can access
    public static bool Ending = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"FakeDoor initialized. Current Ending state: {Ending}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {

            // Set the static Ending boolean to true
            Ending = true;
            Debug.Log("FakeDoor triggered! Ending set to true.");
        }
    }
    
    // Static method to reset the Ending state (useful for testing or restarting)
    public static void ResetEnding()
    {
        Ending = false;
        Debug.Log("FakeDoor: Ending state reset to false.");
    }
    
    // Static method to check the Ending state from other scripts
    public static bool IsEnding()
    {
        return Ending;
    }
}
