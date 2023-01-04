using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusMinusButton : MonoBehaviour
{
    public Button plusButton;
    public Button minusButton;
    public Text valueText;
    public int minValue = 0;
    public int maxValue = 10;

    private int currentValue;

    void Start()
    {
        currentValue = minValue;
        UpdateValueText();

        plusButton.onClick.AddListener(OnPlusButtonClick);
        minusButton.onClick.AddListener(OnMinusButtonClick);
    }

    void OnPlusButtonClick()
    {
        currentValue = Mathf.Min(currentValue + 1, maxValue);
        UpdateValueText();
    }

    void OnMinusButtonClick()
    {
        currentValue = Mathf.Max(currentValue - 1, minValue);
        UpdateValueText();
    }

    void UpdateValueText()
    {
        valueText.text = currentValue.ToString();
    }
}
