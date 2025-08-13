using System;
using System.Collections;
using UnityEngine;

public class AchievementTestInput : MonoBehaviour
{
    public AchievementPopupUI popupUI;

    // 成就文字
    public string collectable1Text;
    public string collectable2Text;
    public string collectable3Text;
    public string collectable4Text;
    public string collectable5Text;

    public string achievement1Text;
    public string achievement2Text;
    public string achievement3Text;
    public string achievement4Text;
    public string achievement5Text;
    public string achievement6Text;
    public string achievement7Text;
    public string achievement8Text;
    public string achievement9Text;
    public string achievement10Text;
    public string achievement11Text;
    public string achievement12Text;
    public string achievement13Text;
    public string achievement14Text;
    public string achievement15Text;

    public static bool c1 = false;
    public static bool c2 = false;
    public static bool c3 = false;
    public static bool c4 = false;
    public static bool c5 = false;
    public static bool a1 = false;
    public static bool a2 = false;
    public static bool a3 = false;
    public static bool a4 = false;
    public static bool a5 = false;
    public static bool a6 = false;
    public static bool a7 = false;
    public static bool a8 = false;
    public static bool a9 = false;
    public static bool a10 = false;
    public static bool a11 = false;
    public static bool a12 = false;
    public static bool a13 = false;
    public static bool a14 = false;
    public static bool a15 = false;

    // collectables
    public Sprite sprite_c1;
    public Sprite sprite_c2;
    public Sprite sprite_c3;
    public Sprite sprite_c4;
    public Sprite sprite_c5;
    //achievements
    public Sprite sprite_a1;
    public Sprite sprite_a2;
    public Sprite sprite_a3;
    public Sprite sprite_a4;
    public Sprite sprite_a5;
    public Sprite sprite_a6;
    public Sprite sprite_a7;
    public Sprite sprite_a8;
    public Sprite sprite_a9;
    public Sprite sprite_a10;
    public Sprite sprite_a11;
    public Sprite sprite_a12;
    public Sprite sprite_a13;
    public Sprite sprite_a14;
    public Sprite sprite_a15;

    void Update()
    {
        if (c1)
        {
            popupUI.ShowPopup(collectable1Text, sprite_c1);
            StartCoroutine(SetBoolToFalseAfterDelay("c1"));
        }
        else if (c2)
        {
            popupUI.ShowPopup(collectable2Text, sprite_c2);
            StartCoroutine(SetBoolToFalseAfterDelay("c2"));
        }
        else if (c3)
        {
            popupUI.ShowPopup(collectable3Text, sprite_c3);
            StartCoroutine(SetBoolToFalseAfterDelay("c3"));
        }
        else if (c4)
        {
            popupUI.ShowPopup(collectable4Text, sprite_c4);
            StartCoroutine(SetBoolToFalseAfterDelay("c4"));
        }
        else if (c5)
        {
            popupUI.ShowPopup(collectable5Text, sprite_c5);
            StartCoroutine(SetBoolToFalseAfterDelay("c5"));
        }

        //acheivements
        else if (a1)
        {
            popupUI.ShowPopup(achievement1Text, sprite_a1);
            StartCoroutine(SetBoolToFalseAfterDelay("a1"));
        }
        else if (a2)
        {
            popupUI.ShowPopup(achievement2Text, sprite_a2);
            StartCoroutine(SetBoolToFalseAfterDelay("a2"));
        }
        else if (a3)
        {
            popupUI.ShowPopup(achievement3Text, sprite_a3);
            StartCoroutine(SetBoolToFalseAfterDelay("a3"));
        }
        else if (a4)
        {
            popupUI.ShowPopup(achievement4Text, sprite_a4);
            StartCoroutine(SetBoolToFalseAfterDelay("a4"));
        }
        else if (a5)
        {
            popupUI.ShowPopup(achievement5Text, sprite_a5);
            StartCoroutine(SetBoolToFalseAfterDelay("a5"));
        }
        else if (a6)
        {
            popupUI.ShowPopup(achievement6Text, sprite_a6);
            StartCoroutine(SetBoolToFalseAfterDelay("a6"));
        }
        else if (a7)
        {
            popupUI.ShowPopup(achievement7Text, sprite_a7);
            StartCoroutine(SetBoolToFalseAfterDelay("a7"));
        }
        else if (a8)
        {
            popupUI.ShowPopup(achievement8Text, sprite_a8);
            StartCoroutine(SetBoolToFalseAfterDelay("a8"));
        }
        else if (a9)
        {
            popupUI.ShowPopup(achievement9Text, sprite_a9);
            StartCoroutine(SetBoolToFalseAfterDelay("a9"));
        }
        else if (a10)
        {
            popupUI.ShowPopup(achievement10Text, sprite_a10);
            StartCoroutine(SetBoolToFalseAfterDelay("a10"));
        }
        else if (a11)
        {
            popupUI.ShowPopup(achievement11Text, sprite_a11);
            StartCoroutine(SetBoolToFalseAfterDelay("a11"));
        }
        else if (a12)
        {
            popupUI.ShowPopup(achievement12Text, sprite_a12);
            StartCoroutine(SetBoolToFalseAfterDelay("a12"));
        }
        else if (a13)
        {
            popupUI.ShowPopup(achievement13Text, sprite_a13);
            StartCoroutine(SetBoolToFalseAfterDelay("a13"));
        }
        else if (a14)
        {
            popupUI.ShowPopup(achievement14Text, sprite_a14);
            StartCoroutine(SetBoolToFalseAfterDelay("a14"));
        }
        else if (a15)
        {
            popupUI.ShowPopup(achievement15Text, sprite_a15);
            StartCoroutine(SetBoolToFalseAfterDelay("a15"));
        }
    }
    public void SetBool(string boolName, bool value)
    {
        switch (boolName.ToLower())
        {
            // Collectables
            case "c1":
                c1 = value;
                break;
            case "c2":
                c2 = value;
                break;
            case "c3":
                c3 = value;
                break;
            case "c4":
                c4 = value;
                break;
            case "c5":
                c5 = value;
                break;
            
            // Achievements
            case "a1":
                a1 = value;
                break;
            case "a2":
                a2 = value;
                break;
            case "a3":
                a3 = value;
                break;
            case "a4":
                a4 = value;
                break;
            case "a5":
                a5 = value;
                break;
            case "a6":
                a6 = value;
                break;
            case "a7":
                a7 = value;
                break;
            case "a8":
                a8 = value;
                break;
            case "a9":
                a9 = value;
                break;
            case "a10":
                a10 = value;
                break;
            case "a11":
                a11 = value;
                break;
            case "a12":
                a12 = value;
                break;
            case "a13":
                a13 = value;
                break;
            case "a14":
                a14 = value;
                break;
            case "a15":
                a15 = value;
                break;
            
            default:
                Debug.LogWarning("AchievementTestInput: Unknown boolean name '" + boolName + "'");
                break;
        }
        
        Debug.Log("AchievementTestInput: Set " + boolName + " to " + value);
    }
    
    private IEnumerator SetBoolToFalseAfterDelay(string boolName)
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);
        
        // Set the boolean back to false
        SetBool(boolName, false);
        Debug.Log("AchievementTestInput: Auto-reset " + boolName + " to false after 2 seconds");
    }
}
