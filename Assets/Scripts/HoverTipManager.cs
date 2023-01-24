using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 250 ? 250 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        Debug.Log(Input.mousePosition.x);
        Debug.Log(Screen.width/2);
        Vector2 tooltipPosition;
        if (Input.mousePosition.x > Screen.width / 2)
        {
            tooltipPosition = new Vector2(Input.mousePosition.x - (tipWindow.sizeDelta.x/3), Input.mousePosition.y);
        }
        else
        {
            tooltipPosition = new Vector2(Input.mousePosition.x + (tipWindow.sizeDelta.x/3), Input.mousePosition.y);
        }
        tipWindow.transform.position = tooltipPosition;
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
