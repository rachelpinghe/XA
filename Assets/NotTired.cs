using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotTired : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToActivate; // GameObject to activate for 3 seconds
    public float activationDuration = 3f; // How long to keep it active
    
    private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();
        
        if (button != null)
        {
            // Add listener to button onClick event
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("NotTired: Button listener added successfully");
        }
        else
        {
            Debug.LogWarning("NotTired: No Button component found on this GameObject!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnButtonClick()
    {
        if (objectToActivate != null)
        {
            StartCoroutine(ActivateObjectTemporarily());
        }
        else
        {
            Debug.LogWarning("NotTired: Object to activate is not assigned!");
        }
    }
    
    private IEnumerator ActivateObjectTemporarily()
    {
        // Activate the object
        objectToActivate.SetActive(true);
        Debug.Log($"NotTired: Activated {objectToActivate.name} for {activationDuration} seconds");
        
        // Wait for the specified duration
        yield return new WaitForSeconds(activationDuration);
        
        // Deactivate the object
        objectToActivate.SetActive(false);
        Debug.Log($"NotTired: Deactivated {objectToActivate.name} after {activationDuration} seconds");
    }
}
