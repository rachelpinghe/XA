using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassDoor : MonoBehaviour
{
    public bool setActive = false;
    public string nextLevelName = "Level2"; // Name of the next level to load
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform activatedChild = transform.Find("Activated");
            if (activatedChild != null)
            {
                setActive = true;
                activatedChild.gameObject.SetActive(setActive); // Activate the "Activated" child GameObject
                
                // Load level two after activating the door
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.LogWarning("PassDoor: Activated child not found!");
            }
        }
    }
}
