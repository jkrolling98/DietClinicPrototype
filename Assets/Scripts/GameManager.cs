using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject patient;
    public TextMeshProUGUI patientInfo;

    public GameObject mealWindow;

    public GameObject summaryWindow;

    // Start is called before the first frame update
    void Start()
    {
        NewPatient();
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
        pickers = mealWindow.GetComponentsInChildren<PlusMinusButton>();
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
            mealWindow.SetActive(false);
            summaryWindow.GetComponentInChildren<Text>().text = "meal served, good job";
            TabManager.instance.ViewSummary();
        }
    }

    public void ResetPickers()
    {
        PlusMinusButton[] pickers;
        pickers = mealWindow.GetComponentsInChildren<PlusMinusButton>();
        for (int i = 0; i < pickers.Length; i++)
        {
            pickers[i].currentValue = 0;
            pickers[i].UpdateValueText();
        }
    }

    public void NewPatient()
    {
        ResetPickers();
        TabManager.instance.ViewHelp();
        //instantiate new patient and update patient info tab
        Patient patient = PatientFactory.instance.CreateNewStudent();
        patientInfo.text = $"Name: {patient.patientName}\n" +
            $"Age: {patient.age} \n" +
            $"Weight: {patient.weight} \n" +
            $"Height: {patient.height} \n\n" +
            $"Occupation: {patient.occupation} \n\n" +
            $"FoodPreference: {patient.preference} \n" +
            $"Allergies: {patient.allergies}";
        Debug.Log(patient.meals);
        Debug.Log(patient.meals[0].dishName);
        Debug.Log(patient.meals[1].dishName);
    }
}
