using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public GameObject conversationUI; // Reference to the UI element for conversation
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
        if (other.CompareTag("Player"))
        {
            conversationUI.SetActive(true); // Show conversation UI when player enters trigger
            StartCoroutine(HideConversationAfterDelay());
        }
    }
    
    private IEnumerator HideConversationAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        
        if (conversationUI != null)
        {
            conversationUI.SetActive(false); // Hide conversation UI after 2 seconds
            Debug.Log("Conversation: UI hidden after 2 seconds");
        }
    }
}
