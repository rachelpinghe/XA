using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallBook : MonoBehaviour
{
    [Header("Fall Settings")]
    public float triggerPlayerX = 5f; // X position where player triggers the book fall
    public float destroyAtY = -10f; // Y position where book destroys itself
    public float pushForce = 2f; // How much force to apply to make book fall forward
    
    [Header("Player Detection")]
    public string playerTag = "Player"; // Tag of the player object
    
    private bool hasFallen = false; // Prevent multiple falls
    private GameObject player;
    private Rigidbody bookRigidbody;
    private Vector3 originalPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        bookRigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        
        if (bookRigidbody == null)
        {
            Debug.LogWarning("FallBook: No Rigidbody found on book object!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only check if Level3 scene is loaded
        if (!SceneManager.GetSceneByName("Level3").isLoaded)
        {
            return;
        }
        
        // Find player if not already found
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                Debug.Log("FallBook: Found player in Level3 scene");
            }
        }
        
        // Check if book should fall
        if (!hasFallen && player != null && player.transform.position.x >= triggerPlayerX)
        {
            FallOffShelf();
        }
        
        // Check if book should be destroyed
        if (hasFallen && transform.position.y <= destroyAtY)
        {
            DestroyBook();
        }
    }
    
    void FallOffShelf()
    {
        if (bookRigidbody != null && !hasFallen)
        {
            hasFallen = true;
            
            // Move the book slightly forward to make it fall off the shelf
            Vector3 pushDirection = transform.forward; // Push in the forward direction
            bookRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            
            // Optional: Add a slight rotation force for more natural falling
            Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            bookRigidbody.AddTorque(torque, ForceMode.Impulse);
            
            Debug.Log($"FallBook: Book falling! Player reached X position: {player.transform.position.x}");
        }
    }
    
    void DestroyBook()
    {
        Debug.Log($"FallBook: Book reached Y position {transform.position.y}, destroying book");
        Destroy(gameObject);
    }
    
    // Public method to manually trigger fall (if needed)
    public void TriggerFall()
    {
        if (!hasFallen)
        {
            FallOffShelf();
        }
    }
    
    // Public method to reset book position (if needed)
    public void ResetBook()
    {
        if (bookRigidbody != null)
        {
            hasFallen = false;
            transform.position = originalPosition;
            transform.rotation = Quaternion.identity;
            bookRigidbody.velocity = Vector3.zero;
            bookRigidbody.angularVelocity = Vector3.zero;
            Debug.Log("FallBook: Book reset to original position");
        }
    }
}
