using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnd : MonoBehaviour
{
    [Header("Activation Settings")]
    public GameObject objectToActivate; // The GameObject to activate when ending is true
    public float activationDuration = 8f; // How long to keep this object active (in seconds)
    
    private bool wasEndingTrue = false; // Track previous ending state
    private bool isActivated = false; // Prevent multiple activations
    
    // Start is called before the first frame update
    void Start()
    {
        // Start with the target object deactivated if it's assigned
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if FakeDoor.Ending is true
        bool isEndingTrue = FakeDoor.Ending;
        
        // Check if ending state changed from false to true
        if (isEndingTrue && !wasEndingTrue && !isActivated)
        {
            ActivateObject();
        }
        
        wasEndingTrue = isEndingTrue;
    }
    
    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            isActivated = true;
            StartCoroutine(DeactivateAfterDelay());
            Debug.Log("ActivateEnd: Activated " + objectToActivate.name + " for " + activationDuration + " seconds");
        }
        else
        {
            Debug.LogWarning("ActivateEnd: Object to activate is not assigned!");
        }
    }
    
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDuration);
        
        // Deactivate the target GameObject
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
            Debug.Log("ActivateEnd: Deactivated " + objectToActivate.name + " after " + activationDuration + " seconds");
        }
        
        // Reset activation flag after a brief delay to allow for new triggers
        yield return new WaitForSeconds(0.5f);
        isActivated = false;
    }
}
