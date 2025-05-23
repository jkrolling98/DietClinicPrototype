using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverTipManager : MonoBehaviour
{
    public Text tipText;
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
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 100 ? 100 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        //float pivotX = mousePos.x / Screen.width;
        //float pivotY = mousePos.y / Screen.height;
        //tipWindow.pivot = new Vector2(pivotX, pivotY);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x / 2, mousePos.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
