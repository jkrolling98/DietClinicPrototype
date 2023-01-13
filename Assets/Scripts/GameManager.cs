using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject patient;
    public TextMeshProUGUI patientInfo;
    public GameObject mealWindow;
    public GameObject pastMealWindow;
    public GameObject pastMealTemplate;
    public GameObject summaryWindow;
    public GameObject dishWindow;
    public GameObject dishTemplate;
    public GameObject wholeGrainsBar;
    public GameObject proteinBar;
    public GameObject fruitVeggieBar;

    private List<Meal> allDishes;

    // Start is called before the first frame update
    void Start()
    {
        NewPatient();
        allDishes = Resources.LoadAll<Meal>("Meals").ToList();
        SetDishWindow();
    }

    public void SetDishWindow()
    {
        foreach (Meal dish in allDishes)
        {
            GameObject dishItem = Instantiate(dishTemplate, dishWindow.transform);
            dishItem.GetComponent<Image>().sprite = dish.image;
            dishItem.GetComponent<HoverTip>().tipToShow = dish.dishName;
            dishItem.SetActive(true);
        }
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

    public void ResetPastMeals()
    {
        if(pastMealWindow.transform.childCount > 1)
        {
            for (int i = pastMealWindow.transform.childCount - 1; i >= 1; i--)
            {
                Debug.Log(pastMealWindow.transform.GetChild(i));
                GameObject.Destroy(pastMealWindow.transform.GetChild(i).gameObject);
            }
        }
        ResetNutrientBar();
    }

    public void ResetNutrientBar()
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current = 0;
        proteinBar.GetComponent<ProgressBar>().current = 0;
        fruitVeggieBar.GetComponent<ProgressBar>().current = 0;
    }

    public void NewPatient()
    {
        ResetPickers();
        ResetPastMeals();
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
        for (int i = 0; i < patient.meals.Length; i++)
        {
            Meal meal = patient.meals[i];
            GameObject pastMeal = Instantiate(pastMealTemplate, pastMealWindow.transform);
            pastMeal.GetComponent<Image>().sprite = meal.image;
            pastMeal.GetComponent<HoverTip>().tipToShow = meal.dishName;
            pastMeal.SetActive(true);
            wholeGrainsBar.GetComponent<ProgressBar>().current += meal.wholeGrainServings;
            proteinBar.GetComponent<ProgressBar>().current += meal.proteinServings;
            fruitVeggieBar.GetComponent<ProgressBar>().current += meal.veggieServings;
        }
    }
}
