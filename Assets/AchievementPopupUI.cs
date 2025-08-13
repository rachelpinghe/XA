using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject popupPanel;    // 弹窗整体Panel
    public TMP_Text popupText;       // 弹窗文字
    public Image popupIcon;          // 弹窗图标

    // 显示弹窗，传入文字和图标
    public void ShowPopup(string message, Sprite icon)
    {
        popupText.text = message;

        if (icon != null)
        {
            popupIcon.sprite = icon;
            popupIcon.gameObject.SetActive(true);
        }
        else
        {
            popupIcon.gameObject.SetActive(false);
        }

        popupPanel.SetActive(true);

        // 3秒后自动隐藏弹窗
        StartCoroutine(HideAfterSeconds(3f));
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        popupPanel.SetActive(false);
    }
}
