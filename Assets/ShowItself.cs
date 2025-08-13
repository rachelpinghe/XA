using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItself : MonoBehaviour
{
    private bool hasBeenActivated = false; // Track if we've already activated once
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FakeDoor.IsEnding() && !hasBeenActivated)
        {
            // Show this GameObject for 3 seconds when Ending becomes true
            StartCoroutine(ShowFor3Seconds());
            hasBeenActivated = true; // Prevent multiple activations
        }
    }
    
    IEnumerator ShowFor3Seconds()
    {
        gameObject.SetActive(true);
        Debug.Log("ShowItself: GameObject activated for 3 seconds.");
        
        yield return new WaitForSeconds(3f);
        
        gameObject.SetActive(false);
        Debug.Log("ShowItself: GameObject deactivated after 3 seconds.");
    }
}
