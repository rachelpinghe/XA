using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassDoor : MonoBehaviour
{
    public bool setActive = false;
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
            }
            else
            {
                Debug.LogWarning("PassDoor: Activated child not found!");
            }
        }
    }
}
