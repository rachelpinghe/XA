using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectStar : MonoBehaviour
{
    [Header("Score Settings")]
    public TextMeshPro scoreText; // Reference to the score UI text (TextMeshPro)
    public int scoreValue = 1; // Points per star
    
    private int currentScore = 0; // Current score counter
    
    void Start()
    {
        // Initialize score display
        UpdateScoreDisplay();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            // Increase score
            currentScore += scoreValue;
            
            // Update score display
            UpdateScoreDisplay();
            
            // Add code to collect the star
            Destroy(other.gameObject);
            
            Debug.Log($"Star collected! Score: {currentScore}");
        }
    }
    
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
        else
        {
            Debug.LogWarning("CollectStar: Score Text is not assigned!");
        }
    }
    
    // Public method to get current score
    public int GetScore()
    {
        return currentScore;
    }
    
    // Public method to reset score
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }
}
