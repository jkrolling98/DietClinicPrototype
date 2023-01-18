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
    public TextMeshProUGUI summaryText;
    public GameObject dishWindow;
    public GameObject dishTemplate;
    public GameObject wholeGrainsBar;
    public GameObject proteinBar;
    public GameObject fruitVeggieBar;

    private List<Meal> allDishes;
    private int initialGrainValue;
    private int initialProteinValue;
    private int initialFruitValue;

    public int score = 0;

    public float timerDuration = 60f;
    public TextMeshProUGUI timerText;
    public GameObject timerBar;
    private float currentTime;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        NewPatient();
        allDishes = Resources.LoadAll<Meal>("Meals").ToList();
        SetDishWindow();
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            timerBar.GetComponent<ProgressBar>().current = (int)currentTime/1;
            timerText.text = currentTime.ToString("F0")+"s";
            if (currentTime <= 0)
            {
                isRunning = false;
                OnTimerFinished();
            }
        }
    }

    public void OnTimerFinished()
    {
        Debug.Log("times up! gameover...");
        summaryText.text = "Times up! Gameover!";
        TabManager.instance.ViewSummary();
        ResetDishWindow();
    }

    public void SetDishWindow()
    {
        foreach (Meal dish in allDishes)
        {
            GameObject dishItem = Instantiate(dishTemplate, dishWindow.transform);
            dishItem.name = dish.dishName;
            dishItem.GetComponent<Image>().sprite = dish.image;
            string tooltiptext = dish.dishName + "\n\n" +
                $"Grain Serving: {dish.wholeGrainServings}\n" +
                $"Protein Serving: {dish.proteinServings}\n" +
                $"Fruits n Veggie Serving: {dish.veggieServings}";
            dishItem.GetComponent<HoverTip>().tipToShow = tooltiptext;
            dishItem.SetActive(true);
        }
    }

    public void SelectDish(GameObject dish)
    {
        string path = "Meals/" + dish.name.Replace(" ", "");
        Meal currentDish = Resources.Load<Meal>(path);
        // update seperate values
        if (dish.GetComponent<Toggle>().isOn)
        {
            wholeGrainsBar.GetComponent<ProgressBar>().current += currentDish.wholeGrainServings;
            proteinBar.GetComponent<ProgressBar>().current += currentDish.proteinServings;
            fruitVeggieBar.GetComponent<ProgressBar>().current += currentDish.veggieServings;
        }
        else
        {
            wholeGrainsBar.GetComponent<ProgressBar>().current -= currentDish.wholeGrainServings;
            proteinBar.GetComponent<ProgressBar>().current -= currentDish.proteinServings;
            fruitVeggieBar.GetComponent<ProgressBar>().current -= currentDish.veggieServings;
        }
    }


    public void Serve()
    {
        double grainsScore = (double)wholeGrainsBar.GetComponent<ProgressBar>().current / (double)wholeGrainsBar.GetComponent<ProgressBar>().maximum;
        double proteinScore = (double)proteinBar.GetComponent<ProgressBar>().current / (double)proteinBar.GetComponent<ProgressBar>().maximum;
        double fruitScore = (double)fruitVeggieBar.GetComponent<ProgressBar>().current / (double)fruitVeggieBar.GetComponent<ProgressBar>().maximum;
        Debug.Log("grainScore: "+ System.String.Format("{0:0.00}", grainsScore));
        string summary = $"Grain score : {grainsScore.ToString("0.00")}\n" +
            $"proteinScore : {proteinScore.ToString("0.00")}\n" +
            $"fruitnVeggies Score : {fruitScore.ToString("0.00")}";
        summaryText.text = summary; 
        TabManager.instance.ViewSummary();
        ResetDishWindow();
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
                //Debug.Log(pastMealWindow.transform.GetChild(i));
                GameObject.Destroy(pastMealWindow.transform.GetChild(i).gameObject);
            }
        }
        ResetNutrientBar();
    }

    public void ResetDishWindow()
    {
        for (int i = dishWindow.transform.childCount - 1; i >= 1; i--)
        {
            //Debug.Log(dishWindow.transform.GetChild(i));
            dishWindow.transform.GetChild(i).GetComponent<Toggle>().isOn=false;
        }
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
        for (int i = 0; i < patient.meals.Length; i++)
        {
            Meal meal = patient.meals[i];
            GameObject pastMeal = Instantiate(pastMealTemplate, pastMealWindow.transform);
            pastMeal.name = meal.dishName;
            pastMeal.GetComponent<Image>().sprite = meal.image;
            string tooltiptext = meal.dishName + "\n\n" +
                $"Grain Serving: {meal.wholeGrainServings}\n" +
                $"Protein Serving: {meal.proteinServings}\n" +
                $"Fruits n Veggie Serving: {meal.veggieServings}";
            pastMeal.GetComponent<HoverTip>().tipToShow = tooltiptext;
            pastMeal.SetActive(true);
            wholeGrainsBar.GetComponent<ProgressBar>().current += meal.wholeGrainServings;
            proteinBar.GetComponent<ProgressBar>().current += meal.proteinServings;
            fruitVeggieBar.GetComponent<ProgressBar>().current += meal.veggieServings;
        }
        initialGrainValue = wholeGrainsBar.GetComponent<ProgressBar>().current;
        initialProteinValue = proteinBar.GetComponent<ProgressBar>().current;
        initialFruitValue = fruitVeggieBar.GetComponent<ProgressBar>().current;
        currentTime = timerDuration;
        isRunning = true;
    }
}
