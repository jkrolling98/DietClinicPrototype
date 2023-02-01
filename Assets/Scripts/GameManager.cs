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

    private List<Dish> allDishes;
    private int initialGrainValue;
    private int initialProteinValue;
    private int initialFruitValue;

    public int score = 0;
    public double roundCost = 0;
    public double money = 0;
    public TextMeshProUGUI moneyText;

    public float timerDuration = 60f;
    public TextMeshProUGUI timerText;
    public GameObject timerBar;
    private float currentTime;
    private bool isRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        UpdateMoney();
        NewPatient();
        allDishes = DishManager.instance.GetDishes();
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

    public void UpdateMoney()
    {
        moneyText.text = money.ToString("0.00");
    }

    public void SetDishWindow()
    {
        foreach (Dish dish in allDishes)
        {
            GameObject dishItem = Instantiate(dishTemplate, dishWindow.transform);
            dishItem.name = dish.dishName;
            dishItem.GetComponent<Image>().sprite = dish.image;
            dishItem.GetComponent<HoverTip>().tipToShow = GetDishTooltipText(dish);
            dishItem.SetActive(true);
            Dish dishComponent = dishItem.AddComponent<Dish>();
            dishComponent.setDishData(dish);
        }
    }

    public void SelectDish(GameObject dish)
    {
        Dish currentDish = dish.GetComponent<Dish>();
        // update seperate values
        if (isRunning)
        {
            if (dish.GetComponent<Toggle>().isOn)
            {
                AddProgressBars(currentDish);
                money -= currentDish.cost;
                roundCost += currentDish.cost;
                UpdateMoney();
                UpdateDishBtns();
                Debug.Log($"Called by {currentDish.dishName} to + round cost");
            }
            else
            {
                MinusProgressBars(currentDish);
                money += currentDish.cost;
                roundCost -= currentDish.cost;
                UpdateMoney();
                UpdateDishBtns();
                Debug.Log($"Called by {currentDish.dishName} to - round cost");
            }
        }
    }

    public void UpdateDishBtns()
    {
        for (int i = dishWindow.transform.childCount - 1; i >= 1; i--)
        {
            //Debug.Log(dishWindow.transform.GetChild(i));
            if(dishWindow.transform.GetChild(i).GetComponent<Toggle>().isOn == false)
            {
                dishWindow.transform.GetChild(i).GetComponent<Toggle>().interactable = (money >= dishWindow.transform.GetChild(i).GetComponent<Dish>().cost);
            }
        }
    }

    public void AddProgressBars(Dish dish)
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current += dish.wholeGrainServings;
        proteinBar.GetComponent<ProgressBar>().current += dish.proteinServings;
        fruitVeggieBar.GetComponent<ProgressBar>().current += dish.veggieServings;
    }
    public void MinusProgressBars(Dish dish)
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current -= dish.wholeGrainServings;
        proteinBar.GetComponent<ProgressBar>().current -= dish.proteinServings;
        fruitVeggieBar.GetComponent<ProgressBar>().current -= dish.veggieServings;
    }

    public void Serve()
    {
        isRunning = false;
        UpdateMoney();
        //change to current - initial / max
        double grainConsumed = (double)wholeGrainsBar.GetComponent<ProgressBar>().current - initialGrainValue;
        double proteinConsumed = (double)proteinBar.GetComponent<ProgressBar>().current - initialProteinValue;
        double fruitConsumed = (double)fruitVeggieBar.GetComponent<ProgressBar>().current - initialFruitValue;
        double grainsScore = grainConsumed / (double)wholeGrainsBar.GetComponent<ProgressBar>().maximum;
        double proteinScore = proteinConsumed / (double)proteinBar.GetComponent<ProgressBar>().maximum;
        double fruitScore = fruitConsumed / (double)fruitVeggieBar.GetComponent<ProgressBar>().maximum;
        string summary = $"Grain consumed : {grainConsumed.ToString()}\n" +
            $"Protein consumed : {proteinConsumed.ToString()}\n" +
            $"Fruit consumed : {fruitConsumed.ToString()}\n\n" +
            $"Grain score : {grainsScore.ToString("0.00")}\n" +
            $"proteinScore : {proteinScore.ToString("0.00")}\n" +
            $"fruitnVeggies Score : {fruitScore.ToString("0.00")}\n" +
            $"Money spent : ${roundCost.ToString("0.00")}";
        summaryText.text = summary; 
        TabManager.instance.ViewSummary();
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
        UpdateDishBtns();
    }

    public void ResetNutrientBar()
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current = 0;
        proteinBar.GetComponent<ProgressBar>().current = 0;
        fruitVeggieBar.GetComponent<ProgressBar>().current = 0;
    }

    public void NewPatient()
    {
        roundCost = 0;
        money += 10;
        UpdateMoney();
        ResetDishWindow();
        ResetPastMeals();
        TabManager.instance.ViewHelp();
        //instantiate new patient and update patient info tab
        Patient patient = PatientFactory.instance.CreateNewStudent();
        UpdatePatientInfo(patient);
        initialGrainValue = wholeGrainsBar.GetComponent<ProgressBar>().current;
        initialProteinValue = proteinBar.GetComponent<ProgressBar>().current;
        initialFruitValue = fruitVeggieBar.GetComponent<ProgressBar>().current;
        currentTime = timerDuration;
        isRunning = true;
    }

    public void UpdatePatientInfo(Patient patient)
    {
        //update patient info text
        patientInfo.text = $"Name: {patient.patientName}\n" +
                $"Age: {patient.age} \n" +
                $"Weight: {patient.weight} \n" +
                $"Height: {patient.height} \n\n" +
                $"Occupation: {patient.occupation} \n\n" +
                $"FoodPreference: {patient.preference} \n" +
                $"Allergies: {patient.allergies}";
        //update patients past meals
        for (int i = 0; i < patient.meals.Length; i++)
        {
            Dish meal = patient.meals[i];
            GameObject pastMeal = Instantiate(pastMealTemplate, pastMealWindow.transform);
            pastMeal.name = meal.dishName;
            pastMeal.GetComponent<Image>().sprite = meal.image;
            pastMeal.GetComponent<HoverTip>().tipToShow = GetDishTooltipText(meal);
            pastMeal.SetActive(true);
            AddProgressBars(meal);
        }
    }

    public string GetDishTooltipText(Dish dish)
    {
        string tooltiptext = dish.dishName + "\n"+
                $"Cost: ${dish.cost.ToString("0.00")}\n" +
                $"Grain Serving: {dish.wholeGrainServings}\n" +
                $"Protein Serving: {dish.proteinServings}\n" +
                $"Fruits n Veggie Serving: {dish.veggieServings}";
        return tooltiptext;
    }
}