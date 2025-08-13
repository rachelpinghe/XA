using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyText : MonoBehaviour
{
    public TextMeshProUGUI myText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI referenceText; // Reference to another TextMeshProUGUI component for comparison
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = referenceText.text; // Update myText to match referenceText
    }
}
