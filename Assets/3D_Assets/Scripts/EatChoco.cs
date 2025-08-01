using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatChoco : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        // Check if the player clicked on the chocolate
        if (gameObject.CompareTag("Choco"))
        {
            // Find the LifeNumber script and increase life
            LifeNumber lifeScript = FindObjectOfType<LifeNumber>();
            if (lifeScript != null)
            {
                lifeScript.IncreaseLife(1f); // Increase life by 1
                Debug.Log("Choco eaten! Life increased!");
                
                // Optionally destroy the chocolate after eating
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("EatChoco: No LifeNumber script found in scene!");
            }
        }
    }
}
