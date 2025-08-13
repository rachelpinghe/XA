using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeNumber : MonoBehaviour
{
    public float deathFall = -10f;
    public float life = 5f;
    public TextMeshPro lifeText; // Reference to the UI text for displaying life count

    // Start is called before the first frame update
    void Start()
    {
        UpdateLifeDisplay();
    }
    public void UpdateLifeDisplay()
    {
        if (lifeText != null)
        {
            lifeText.text = life.ToString();
        }
    }

    public void IncreaseLife(float amount = 1f)
    {
        life += amount;
        UpdateLifeDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= deathFall && life > 0)
        {
            life--;
            UpdateLifeDisplay();
            
            if (life > 0)
            {
                PlayerDied(); // Only revive if player still has lives
            }
        }
        if (life <= 0)
        {
            StartNewGame(); // Prevent negative life count
        }
    }

    public void PlayerDied()
    {
        SaveRevive.RevivePlayer(gameObject);
    }

    void StartNewGame()
    {
        life = 5f; // Reset life count
        transform.position = SaveRevive.defaultStartingPosition; // Use default starting position instead
        SideScrollingCamera camera = FindObjectOfType<SideScrollingCamera>();
        if (camera != null)
        {
            camera.RelocateToPlayer();
        }

        // Reset PassDoor script's setActive boolean
        PassDoor passDoor = FindObjectOfType<PassDoor>();
        if (passDoor != null)
        {
            passDoor.setActive = false;
        }

        // Reset all checkpoint states
        SaveRevive[] allCheckpoints = FindObjectsOfType<SaveRevive>();
        foreach (SaveRevive checkpoint in allCheckpoints)
        {
            checkpoint.ResetCheckpointState();
        }

        UpdateLifeDisplay();
        SaveRevive.ResetCheckpoints();
        
        // Reset falling blocks when starting new game
        // Fall.ResetAllFallingBlocks();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            // Check if it's a falling block with disabled Fall script
            Fall fallScript = collision.gameObject.GetComponent<Fall>();
            if (fallScript != null && !fallScript.enabled)
            {
                return; // Don't take damage if Fall script is disabled
            }

            life--; // Decrease life count when hitting a spike
            UpdateLifeDisplay();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            life--; // Decrease life count when hitting an enemy
            UpdateLifeDisplay();
        }
        if (collision.gameObject.CompareTag("Spike2"))
        {
            // Check if it's a falling block with disabled Fall script
            Fall fallScript = collision.gameObject.GetComponent<Fall>();
            if (fallScript != null && !fallScript.enabled)
            {
                return; // Don't take damage if Fall script is disabled
            }

            life = 1; // Decrease life count when hitting an enemy
            UpdateLifeDisplay();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            life = 0;
            UpdateLifeDisplay();
        }
    }
}
