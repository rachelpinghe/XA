using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetTrue : MonoBehaviour
{
    private Button bed;
    private AchievementTestInput achievementScript; // Direct reference to the script component
    public bool isButton;
    public bool isTrigger;
    public bool isCollider;
    public string boolName = "";
    
    // Start is called before the first frame update
    void Start()
    {
        // Find the AchievementTestInput when this script starts
        FindAchievementTestInput();
        
        // Subscribe to scene loaded events in case SampleScene loads later
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if (isButton)
        {
            bed = GetComponent<Button>();
            bed.onClick.AddListener(OnBedClick);
        }
        else if (isTrigger)
        {
        }
        else if (isCollider)
        {
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When any scene loads, try to find the AchievementTestInput
        if (scene.name == "SampleScene" || achievementScript == null)
        {
            FindAchievementTestInput();
        }
    }
    
    void FindAchievementTestInput()
    {
        // Find GameObject by name across all loaded scenes
        GameObject achievementObject = GameObject.Find("AchievementTestInput");
        
        if (achievementObject != null)
        {
            achievementScript = achievementObject.GetComponent<AchievementTestInput>();
            if (achievementScript != null)
            {
                Debug.Log("SetTrue: Found AchievementTestInput script successfully");
            }
            else
            {
                Debug.LogWarning("SetTrue: Found GameObject 'AchievementTestInput' but no AchievementTestInput script component");
            }
        }
        else
        {
            Debug.LogWarning("SetTrue: Could not find GameObject named 'AchievementTestInput'");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBedClick()
    {
        if (achievementScript != null)
        {
            achievementScript.SetBool(boolName, true);
            Destroy(gameObject); // Destroy the button after clicking
        }
        else
        {
            Debug.LogWarning("SetTrue: AchievementTestInput script not found when trying to set " + boolName);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (achievementScript != null)
            {
                achievementScript.SetBool(boolName, true);
                Debug.Log($"Trigger entered by {other.name}, setting {boolName} to true.");
                if (isTrigger)
                {
                    Destroy(gameObject); // Optionally destroy the object after triggering
                }
            }
            else
            {
                Debug.LogWarning("SetTrue: AchievementTestInput script not found when trying to set " + boolName);
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (achievementScript != null)
            {
                achievementScript.SetBool(boolName, true);
                Debug.Log($"Collision with {collision.gameObject.name}, setting {boolName} to true.");
                if (isCollider)
                {
                    Destroy(gameObject); // Optionally destroy the object after collision
                }
            }
            else
            {
                Debug.LogWarning("SetTrue: AchievementTestInput script not found when trying to set " + boolName);
            }
        }
    }
}
