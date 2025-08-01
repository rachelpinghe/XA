using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeNumber : MonoBehaviour
{
    public float deathFall = -10f;
    public float life = 3f;
    public TextMeshPro lifeText; // Reference to the UI text for displaying life count

    // Start is called before the first frame update
    void Start()
    {
        UpdateLifeDisplay();
    }
    void UpdateLifeDisplay()
    {
        if (lifeText != null)
        {
            lifeText.text = life.ToString();
        }
        else
        {
            Debug.LogWarning("LifeNumber: Life Text is not assigned!");
        }
    }

    public void IncreaseLife(float amount = 1f)
    {
        life += amount;
        UpdateLifeDisplay();
        Debug.Log("Life increased! Current life: " + life);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= deathFall)
        {
            PlayerDied();
            Debug.Log("Player fell");
            life--;
            UpdateLifeDisplay();
        }
        if (life <= 0)
        {
            StartNewGame(); // Prevent negative life count
        }
    }

    void PlayerDied()
    {
        SaveRevive.RevivePlayer(gameObject);
    }

    void StartNewGame()
    {
        life = 3f; // Reset life count
        transform.position = SaveRevive.defaultStartingPosition; // Reset player position
        SideScrollingCamera camera = FindObjectOfType<SideScrollingCamera>();
        if (camera != null)
        {
            camera.RelocateToPlayer();
        }
        else
        {
            Debug.LogWarning("SaveRevive: No SideScrollingCamera found in scene!");
        }
        
        // Reset PassDoor script's setActive boolean
        PassDoor passDoor = FindObjectOfType<PassDoor>();
        if (passDoor != null)
        {
            passDoor.setActive = false;
            Debug.Log("StartNewGame: PassDoor setActive reset to false");
        }
        else
        {
            Debug.LogWarning("StartNewGame: No PassDoor script found in scene!");
        }
        
        // Reset all checkpoint states
        SaveRevive[] allCheckpoints = FindObjectsOfType<SaveRevive>();
        foreach (SaveRevive checkpoint in allCheckpoints)
        {
            checkpoint.ResetCheckpointState();
        }
        
        UpdateLifeDisplay();
        SaveRevive.ResetCheckpoints();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            life--; // Decrease life count when hitting a spike
            UpdateLifeDisplay();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            life = life - 2; // Decrease life count when hitting an enemy
            UpdateLifeDisplay();
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            life = 0; // Instant death in water
            UpdateLifeDisplay();
            PlayerDied();
        }
    }
}
