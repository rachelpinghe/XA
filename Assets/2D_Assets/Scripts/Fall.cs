using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [Header("Fall Settings")]
    public float minimumYPosition = -10f; // The lowest Y position the block will fall to
    public float fallSpeed = 10f; // How fast the block falls
    
    [Header("Activation Settings")]
    public GameObject objectToActivate; // GameObject to activate when falling starts
    public float activationDuration = 3f; // How long to keep the object active (in seconds)
    public List<GameObject> gameObjectsToActivate = new List<GameObject>(); // GameObjects to activate on fall
    
    private bool isFalling = false;
    public string waterTag = "Water"; // Tag for water objects
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        
        // If gameObjectsToActivate list is empty, try to find water objects by tag
        if (gameObjectsToActivate.Count == 0)
        {
            // Find objects with both "Water" and "AppearAfterTrigger" tags
            List<GameObject> waterObjectsList = new List<GameObject>();
            waterObjectsList.AddRange(GameObject.FindGameObjectsWithTag(waterTag));
            waterObjectsList.AddRange(GameObject.FindGameObjectsWithTag("AppearAfterTriggered"));
            
            gameObjectsToActivate = waterObjectsList;
        }
        
        // Deactivate all GameObjects at start
        foreach (GameObject obj in gameObjectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log("Deactivated GameObject '" + obj.name + "' at start");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            // Move the block down quickly
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            
            // Stop falling when reaching minimum Y position
            if (transform.position.y <= minimumYPosition)
            {
                Vector3 finalPosition = transform.position;
                finalPosition.y = minimumYPosition;
                transform.position = finalPosition;
                isFalling = false;
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartFalling();
        }
    }

    void StartFalling()
    {
        isFalling = true;
        // Debug.Log("Block started falling!");

        // Activate the specified GameObject for the duration
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            StartCoroutine(DeactivateAfterDelay());
        }

        // Activate all GameObjects in the list
        foreach (GameObject obj in gameObjectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log("Activated GameObject '" + obj.name + "' on fall");
            }
        }
        
        AchievementTestInput.a4 = true; // Set achievement flag for falling blocks
    }
    
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDuration);
        
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
            Debug.Log("Deactivated " + objectToActivate.name + " after " + activationDuration + " seconds");
        }
    }
    
    // Optional: Reset the block to its original position (useful for testing)
    public void ResetBlock()
    {
        transform.position = originalPosition;
        isFalling = false;
        Debug.Log("Block reset to original position");
    }
    
    // Static method to reset all falling blocks when player is revived
    public static void ResetAllFallingBlocks()
    {
        Fall[] allFallingBlocks = FindObjectsOfType<Fall>();
        foreach (Fall fallingBlock in allFallingBlocks)
        {
            fallingBlock.ResetBlock();
        }
        Debug.Log("All falling blocks reset to original positions");
    }
}
