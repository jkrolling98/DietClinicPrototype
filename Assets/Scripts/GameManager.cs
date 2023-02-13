using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject patient;
    public Canvas canvas;
    public TextMeshProUGUI patientInfo;
    public GameObject popUpWindow;
    public GameObject pastMealWindow;
    public GameObject pastMealTemplate;
    public TextMeshProUGUI summaryText;
    public GameObject dishWindow;
    public GameObject dishTemplate;
    public GameObject wholeGrainsBar;
    public GameObject proteinBar;
    public GameObject fruitVeggieBar;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    private List<Dish> allDishes;
    private int initialGrainValue;
    private int initialProteinValue;
    private int initialFruitValue;

    public int score = 0;
    public double roundCost = 0;
    public double money = 0;
    public GameObject moneyText;
    public int level = 1;
    public GameObject levelBar;
    public TextMeshProUGUI levelText;
    public GameObject popUpText;
    public GameObject speechBubble;

    public float timerDuration = 60f;
    public TextMeshProUGUI timerText;
    public GameObject timerBar;
    private float currentTime;
    public static bool isRunning = false;

    public Sprite servingReference;
    public Sprite customer_boy;
    public Sprite customer_girl;
    private Patient currentPatient;
    public List<string> selectedDishes;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        UpdateMoney();
        NewPatient();
        allDishes = DishManager.GetDishes();
        SetDishWindow();
        string headerText = "Welcome to Diet Clinic!";
        string bodyText = "Help our patient find the ideal meal combinations and aim to get as many 3* reviews as possible! \n\nHint: click on the ? icon to view recommended servings!";
        InstantiatePopUp(headerText,bodyText, servingReference);
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

    public void InstantiatePopUp(string headerText, string bodyText, Sprite hintImage = null)
    {
        GameObject popUp = Instantiate(popUpWindow, canvas.transform.position, Quaternion.identity, canvas.transform);
        TextMeshProUGUI header = popUp.transform.Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI body = popUp.transform.Find("BodyText").GetComponent<TextMeshProUGUI>();
        header.text = headerText;
        body.text = bodyText;
        if (hintImage != null)
        {
            Image hint = popUp.transform.Find("HelpWindow").Find("HelpImage").GetComponent<Image>();
            hint.sprite = hintImage;
        }
        isRunning = false;
    }

    public void UpdateMoney()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = $"${money.ToString("0.00")}";
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
                AnimatePopUpText(moneyText,$"-${currentDish.cost.ToString("0.00")}", Color.yellow);
                selectedDishes.Add(currentDish.dishName);
                string res ="";
                foreach(string thisDish in selectedDishes)
                {
                    res += thisDish+"\n";
                }
                Debug.Log(res);
            }
            else
            {
                MinusProgressBars(currentDish);
                money += currentDish.cost;
                roundCost -= currentDish.cost;
                UpdateMoney();
                UpdateDishBtns();
                Debug.Log($"Called by {currentDish.dishName} to - round cost");
                AnimatePopUpText(moneyText, $"+${currentDish.cost.ToString("0.00")}", Color.yellow);
                selectedDishes.Remove(currentDish.dishName);
            }
        }
    }

    public void AnimatePopUpText(GameObject parent, string text, Color color)
    {
        GameObject popUp = Instantiate(popUpText, parent.transform.position, Quaternion.identity, parent.transform);
        popUp.GetComponent<TextMeshProUGUI>().text = text;
        popUp.GetComponent<TextMeshProUGUI>().color = color;
        LeanTween.moveY(popUp, popUp.transform.position.y + 30, .2f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     LeanTween.alpha(popUp, 0, .2f)
                              .setEase(LeanTweenType.easeInOutQuad)
                              .setOnComplete(() =>
                              {
                                  Destroy(popUp);
                              });
                 });

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
        double grainsScore = wholeGrainsBar.GetComponent<ProgressBar>().current>= wholeGrainsBar.GetComponent<ProgressBar>().maximum? 1 : grainConsumed / ((double)wholeGrainsBar.GetComponent<ProgressBar>().maximum-initialGrainValue);
        double proteinScore = proteinBar.GetComponent<ProgressBar>().current >= proteinBar.GetComponent<ProgressBar>().maximum ? 1 : proteinConsumed / ((double)proteinBar.GetComponent<ProgressBar>().maximum-initialProteinValue);
        double fruitScore = fruitVeggieBar.GetComponent<ProgressBar>().current >= fruitVeggieBar.GetComponent<ProgressBar>().maximum ? 1 : (fruitConsumed / ((double)fruitVeggieBar.GetComponent<ProgressBar>().maximum-initialFruitValue));
        string summary = $"Grain consumed : {grainConsumed}\n" +
            $"Protein consumed : {proteinConsumed}\n" +
            $"Fruit consumed : {fruitConsumed}\n\n" +
            $"Grain score : {grainsScore.ToString("0.00")}\n" +
            $"proteinScore : {proteinScore.ToString("0.00")}\n" +
            $"fruitnVeggies Score : {fruitScore.ToString("0.00")}\n" +
            $"Money spent : ${roundCost.ToString("0.00")}";
        double OverallScore = (grainsScore + proteinScore + fruitScore) / 3;
        Debug.Log(OverallScore);
        Debug.Log("checking for repeats");
        foreach(Dish dish in currentPatient.meals)
        {
            Debug.Log(dish.dishName);
            if (selectedDishes.Contains(dish.dishName))
            {
                Debug.Log($"Penalty! {dish.dishName} is repeated.");
                // add penalty
            }
        }
        int starCount = (int)(OverallScore / 0.33);
        PlayStarsAnim(starCount);
        UpdateLevel(starCount);
        switch (starCount)
        {
            case 0:
                UpdateSpeechBubble("That was the worst!");
                break;
            case 1:
                UpdateSpeechBubble("Could be better...");
                break;
            case 2:
                UpdateSpeechBubble("Not too shabby!");
                break;
            case 3:
                UpdateSpeechBubble("That was awesome!");
                break;
            default: break;
        }
        summaryText.text = summary; 
        TabManager.instance.ViewSummary();
    }

    void UpdateLevel(int count)
    {
        levelBar.GetComponent<ProgressBar>().current += count;
        if(levelBar.GetComponent<ProgressBar>().current >= levelBar.GetComponent<ProgressBar>().maximum)
        {
            //int remainder = (levelBar.GetComponent<ProgressBar>().current - levelBar.GetComponent<ProgressBar>().maximum);
            level++;
            levelText.text = (level).ToString();

            levelBar.GetComponent<ProgressBar>().maximum += (int)(level * 0.5 * 10);
            //levelBar.GetComponent<ProgressBar>().current = remainder;
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
        AnimatePopUpText(moneyText, $"+$15.00", Color.yellow);
        money += 15;
        UpdateMoney();
        ResetDishWindow();
        ResetPastMeals();
        ResetStars();
        selectedDishes.Clear();
        TabManager.instance.ViewHelp();
        //instantiate new patient and update patient info tab
        currentPatient = PatientFactory.instance.CreateNewStudent();
        patient.GetComponent<Image>().sprite = currentPatient.gender == Patient.Gender.Male ? customer_boy : customer_girl;
        UpdatePatientInfo(currentPatient);
        initialGrainValue = wholeGrainsBar.GetComponent<ProgressBar>().current;
        initialProteinValue = proteinBar.GetComponent<ProgressBar>().current;
        initialFruitValue = fruitVeggieBar.GetComponent<ProgressBar>().current;
        currentTime = timerDuration;
        UpdateSpeechBubble("Can't wait to eat!");
        isRunning = true;
    }

    public void UpdateSpeechBubble(string text)
    {
        speechBubble.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void UpdatePatientInfo(Patient patient)
    {
        //update patient info text
        patientInfo.text = $"Name: {patient.patientName}\n" +
                $"Age: {patient.age} years old \n" +
                $"Weight: {patient.weight} kg \n" +
                $"Height: {patient.height} cm \n\n" +
                $"Occupation: {patient.occupation} \n\n" +
                $"FoodPreference: {patient.preference} \n" +
                $"Allergies: {patient.allergies}";
        patientInfo.text += $"\n\n" +
            $"Activity Level: {patient.activityLevel}";
        //update patients past meals
        UpdatePastMeals(patient);
    }

    public void UpdatePastMeals(Patient patient)
    {
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

    void PlayStarsAnim(int count)
    {
        if (count >= 1) LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        if (count >= 2) LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
        if (count >= 3) LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.3f).setEase(LeanTweenType.easeOutElastic);
    }

    void ResetStars()
    {
        Debug.Log("Resetting stars");
        Vector3 scaleAmount = new Vector3(0f, 0f, 0f);
        LeanTween.scale(star1, scaleAmount, 2f);
        LeanTween.scale(star2, scaleAmount, 2f);
        LeanTween.scale(star3, scaleAmount, 2f);
    }
}