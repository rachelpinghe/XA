using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecretDoor : MonoBehaviour
{
    [Header("Secret Door Settings")]
    public string secretDoorName = "SecretDoor"; // Name of the secret door to make disappear
    // public bool useRenderer = true; // Disable renderer instead of destroying object
    // public bool useSetActive = false; // Set object inactive instead of disabling renderer
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the player (this object) collided with the secret door
        if (other.gameObject.name == secretDoorName)
        {
            MakeSecretDoorDisappear(other.gameObject);
        }
    }
    
    void MakeSecretDoorDisappear(GameObject door)
    {
        door.SetActive(false);
        Debug.Log("OpenSecretDoor: Secret door '" + door.name + "' set inactive!");
    }
}
