using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealWindowManager : MonoBehaviour
{
    public GameObject mealWindow;
    public GameObject hint;
    public Image mealArt;
    public Text mealText;

    public void OnClick(Object sender)
    {
        Debug.Log(sender);
        if (!mealWindow.activeSelf)
        {
            mealText.text = "Customise your \nBurger";
            mealWindow.SetActive(true);
        }
    }

    public void OnClose()
    {
        mealWindow.SetActive(false);
    }

    public void toggleHelp()
    {
        hint.SetActive(!hint.activeSelf);
    }
}
