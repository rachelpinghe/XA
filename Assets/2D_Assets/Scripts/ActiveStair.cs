using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStair : MonoBehaviour
{
    private static ActiveStair instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Static method to activate the stair from other scenes
    public static void ActivateStair()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(true);
            Debug.Log("ActiveStair: Stair activated from 3D scene water click!");
        }
        else
        {
            Debug.LogWarning("ActiveStair: No instance found to activate!");
        }
    }
}
