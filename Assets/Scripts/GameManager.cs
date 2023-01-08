using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject patient;
    public Text patientInfo;

    public GameObject MealWindow;

    public GameObject SummaryWindow;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate new patient and update patient info tab
    }

    public void Serve()
    {
        //int valueA = int.Parse(IngredientA.GetComponent<Text>().text);
        //int valueB = int.Parse(IngredientB.GetComponent<Text>().text);
        //int valueC = int.Parse(IngredientC.GetComponent<Text>().text);
        //MealWindow.ge

        //if (valueA + valueB + valueC < 1)
        //{
        //    Debug.Log("insufficient Ingredients!");
        //}
        //else
        //{
        //    Debug.Log("Dish served!");
        //}
        PlusMinusButton[] pickers;
        pickers = MealWindow.GetComponentsInChildren<PlusMinusButton>();
        int sum = 0;
        for (int i = 0; i < pickers.Length; i++)
        {
            Debug.Log($"Ingredient {i + 1} value: {pickers[i].currentValue}");
            sum+= pickers[i].currentValue;
        }
        if (sum < 1)
        {
            Debug.Log("Please select at least one ingredient!");
        }
        else
        {
            Debug.Log("Success!");
            MealWindow.SetActive(false);
            SummaryWindow.GetComponentInChildren<Text>().text = "meal served, good job";
            TabManager.Instance.ViewSummary();
        }
    }

    public void ResetPickers()
    {
        PlusMinusButton[] pickers;
        pickers = MealWindow.GetComponentsInChildren<PlusMinusButton>();
        for (int i = 0; i < pickers.Length; i++)
        {
            pickers[i].currentValue= 0;
        }
    }

    public void NewPatient()
    {
        ResetPickers();
        TabManager.Instance.ViewHelp();
    }
}
