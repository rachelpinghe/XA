using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoasMode : MonoBehaviour
{
    [Header("Chaos Settings")]
    public GameObject chaosGameObject; // The GameObject to activate when chaos mode triggers
    public bool destroyTriggerAfterUse = true; // Whether to destroy this trigger after activation
    
    // Start is called before the first frame update
    void Start()
    {
        // Make sure the chaos GameObject is initially inactive
        if (chaosGameObject != null)
        {
            chaosGameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            chaosGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
