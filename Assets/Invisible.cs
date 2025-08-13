using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
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
        if (other.CompareTag("Invisible"))
        {
            // Make the player invisible when they enter the trigger
            Renderer playerRenderer = other.GetComponent<SpriteRenderer>();
            if (playerRenderer != null)
            {
                playerRenderer.enabled = false; // Hide the player
                Debug.Log("Player is now invisible!");
            }
            else
            {
                Debug.LogWarning("Invisible: No Renderer found on Player!");
            }
        }
    } 
}
