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
    public GameObject wholeGrainPortionUI;
    public GameObject proteinPortionUI;
    public GameObject veggiePortionUI;
    public GameObject popUpText;
    public GameObject speechBubble;
    public GameObject moneyText;
    public GameObject costText;
    public GameObject calorieText;
    public GameObject starText;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public static List<Dish> allDishes;
    private int initialGrainValue;
    private int initialProteinValue;
    private int initialFruitValue;

    // round related variables
    public int stars = 0;
    public int roundWholeGrainServing = 0;
    private int roundProteinServing = 0;
    public int roundVeggieServing = 0;
    public double roundCost = 0;
    public double money = 0;
    public int level = 1;
    public GameObject levelBar;
    public TextMeshProUGUI levelText;
    public int roundCalorie = 0;
    
    private Patient currentPatient;
    public List<Dish> selectedDishes;

    public float timerDuration = 60f;
    public TextMeshProUGUI timerText;
    public GameObject timerBar;
    private float currentTime;
    public static bool isRunning = false;

    public Sprite servingReference;
    public Sprite customer_boy;
    public Sprite customer_girl;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        UpdateMoney();
        NewPatient();
        allDishes = DishManager.GetDishes();
        SetDishWindow();
        string headerText = "Welcome to Diet Clinic";
        string bodyText = "Help our patient find the ideal meal combinations and aim to get as many 3* reviews as possible!";
        string footerText = "Hint: click on the ? icon to view recommended servings!";
        InstantiatePopUp(headerText,bodyText, footerText, servingReference);
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            timerBar.GetComponent<ProgressBar>().current = (int)currentTime/1;
            timerText.text = currentTime>60? $"{((int)currentTime/60).ToString("F0")}m {((int)currentTime %60).ToString("F0")}s":currentTime.ToString("F0")+"s";
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

    public void InstantiatePopUp(string headerText, string bodyText, string footerText, Sprite hintImage = null)
    {
        GameObject popUp = Instantiate(popUpWindow, canvas.transform.position, Quaternion.identity, canvas.transform);
        TextMeshProUGUI header = popUp.transform.Find("PopUpWindow").Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI body = popUp.transform.Find("PopUpWindow").Find("BodyText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI footer = popUp.transform.Find("PopUpWindow").Find("FooterText").GetComponent<TextMeshProUGUI>();
        header.text = headerText;
        body.text = bodyText;
        footer.text = footerText;
        if (hintImage != null)
        {
            Image hint = popUp.transform.Find("PopUpWindow").Find("HelpWindow").Find("HelpImage").GetComponent<Image>();
            hint.sprite = hintImage;
            popUp.transform.Find("PopUpWindow").Find("Header").Find("HintBtn").gameObject.SetActive(true);
        }
        isRunning = false;
    }

    public void UpdateMoney()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = $"${money.ToString("0.00")}";
    }

    public void UpdateCost()
    {
        costText.GetComponent<TextMeshProUGUI>().text = $"${roundCost.ToString("0.00")}";
    }

    public void UpdateCalorie()
    {
        calorieText.GetComponent<TextMeshProUGUI>().text = $"{roundCalorie.ToString()} kcal";
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
                AddDish(currentDish);
                //string res ="";
                //foreach(Dish thisDish in selectedDishes)
                //{
                //    res += thisDish.dishName+"\n";
                //}
                //Debug.Log(res);
            }
            else
            {
                MinusProgressBars(currentDish);
                RemoveDish(currentDish);
                //Debug.Log($"Called by {currentDish.dishName} to - round cost");
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

    public void UpdatePortionUI()
    {
        wholeGrainPortionUI.SetActive(roundWholeGrainServing > 0);
        wholeGrainPortionUI.GetComponentInChildren<TextMeshProUGUI>().text = roundWholeGrainServing.ToString();
        proteinPortionUI.SetActive(roundProteinServing > 0);
        proteinPortionUI.GetComponentInChildren<TextMeshProUGUI>().text = roundProteinServing.ToString();
        veggiePortionUI.SetActive(roundVeggieServing > 0);
        veggiePortionUI.GetComponentInChildren<TextMeshProUGUI>().text = roundVeggieServing.ToString();
    }

    public void AddProgressBars(Dish dish)
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current += dish.wholeGrainServings;
        proteinBar.GetComponent<ProgressBar>().current += dish.proteinServings;
        fruitVeggieBar.GetComponent<ProgressBar>().current += dish.veggieServings;
    }

    public void AddServings(Dish dish)
    {
        roundWholeGrainServing += dish.wholeGrainServings;
        roundProteinServing += dish.proteinServings;
        roundVeggieServing += dish.veggieServings;
        UpdatePortionUI();
    }

    public void MinusProgressBars(Dish dish)
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current -= dish.wholeGrainServings;
        proteinBar.GetComponent<ProgressBar>().current -= dish.proteinServings;
        fruitVeggieBar.GetComponent<ProgressBar>().current -= dish.veggieServings;
    }


    public void RemoveServings(Dish dish)
    {
        roundWholeGrainServing -= dish.wholeGrainServings;
        roundProteinServing -= dish.proteinServings;
        roundVeggieServing -= dish.veggieServings;
        UpdatePortionUI();
    }

    public void AddDish(Dish dish)
    {
        AddServings(dish);
        roundCost += dish.cost;
        UpdateCost();
        AnimatePopUpText(costText, $"+${dish.cost.ToString("0.00")}", Color.yellow);
        roundCalorie += (int)dish.calories;
        UpdateCalorie();
        AnimatePopUpText(calorieText, $"+{dish.calories.ToString()} kcal", Color.yellow);
        selectedDishes.Add(dish);
    }
    public void RemoveDish(Dish dish)
    {
        RemoveServings(dish);
        roundCost -= dish.cost;
        UpdateCost();
        AnimatePopUpText(costText, $"-${dish.cost.ToString("0.00")}", Color.yellow);
        roundCalorie -= (int)dish.calories;
        UpdateCalorie();
        AnimatePopUpText(calorieText, $"-{dish.calories.ToString()} kcal", Color.yellow);
        selectedDishes.Remove(dish);
    }

    public void UpdateSummary()
    {
        double grainConsumed = (double)wholeGrainsBar.GetComponent<ProgressBar>().current - initialGrainValue;
        double proteinConsumed = (double)proteinBar.GetComponent<ProgressBar>().current - initialProteinValue;
        double fruitConsumed = (double)fruitVeggieBar.GetComponent<ProgressBar>().current - initialFruitValue;
        double grainsScore = wholeGrainsBar.GetComponent<ProgressBar>().current >= wholeGrainsBar.GetComponent<ProgressBar>().maximum ? 1 : grainConsumed / ((double)wholeGrainsBar.GetComponent<ProgressBar>().maximum - initialGrainValue);
        double proteinScore = proteinBar.GetComponent<ProgressBar>().current >= proteinBar.GetComponent<ProgressBar>().maximum ? 1 : proteinConsumed / ((double)proteinBar.GetComponent<ProgressBar>().maximum - initialProteinValue);
        double fruitScore = fruitVeggieBar.GetComponent<ProgressBar>().current >= fruitVeggieBar.GetComponent<ProgressBar>().maximum ? 1 : (fruitConsumed / ((double)fruitVeggieBar.GetComponent<ProgressBar>().maximum - initialFruitValue));
        string summary = $"Grain consumed : {grainConsumed}\n" +
            $"Protein consumed : {proteinConsumed}\n" +
            $"Fruit consumed : {fruitConsumed}\n\n" +
            $"Grain score : {grainsScore.ToString("0.00")}\n" +
            $"proteinScore : {proteinScore.ToString("0.00")}\n" +
            $"fruitnVeggies Score : {fruitScore.ToString("0.00")}\n" +
            $"Money spent : ${roundCost.ToString("0.00")}\n\n";
        double OverallScore = (grainsScore + proteinScore + fruitScore) / 3;
        Debug.Log(OverallScore);
        Debug.Log("checking for repeats");
        List<string> penaltyList = new List<string>();
        foreach (Dish dish in selectedDishes)
        {
            if (dish.dishName == currentPatient.meals[0].dishName || dish.dishName == currentPatient.meals[1].dishName)
            {
                Debug.Log($"Penalty! {dish.dishName} is repeated.");
                penaltyList.Add($"{dish.dishName} is repeated.");
                // add penalty
            }
            if (currentPatient.allergies != Patient.Allergies.NIL)
            {
                List<string> triggerlist = new List<string>();
                foreach (Ingredient ingredient in dish.ingredients)
                {
                    if (ingredient.allergenOf.ToString() == currentPatient.allergies.ToString())
                    {
                        triggerlist.Add(ingredient.ingredientName);
                        //penaltyList.Add($"Patient's {currentPatient.allergies.ToString()} Allergy triggered by {ingredient.ingredientName} present in {dish.dishName}.");
                    }
                }
                if (triggerlist.Count != 0)
                {
                    string triggers = "";
                    for (int i =0; i<triggerlist.Count;i++)
                    {
                        if (i > 0)
                        {
                            if (i == triggerlist.Count - 1)
                            {
                                triggers += $" and {triggerlist[i]}";
                            }
                            else
                            {
                                triggers += $", {triggerlist[i]}";
                            }
                        }
                        else
                        {
                            triggers += triggerlist[i];
                        }
                    }
                    penaltyList.Add($"Patient's {currentPatient.allergies.ToString()} Allergy triggered by {triggers} present in {dish.dishName}.");
                }
            }
        }
        if(penaltyList.Count > 0)
        {
            summary += "Penalties\n";
            foreach (string penaltyText in penaltyList)
            {
                summary += penaltyText + "\n";
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
    }

    public void Serve()
    {
        if(roundCost!=0 && roundCost <= money)
        {
            isRunning = false;
            money -= roundCost;
            UpdateMoney();
            //change to current - initial / max
            UpdateSummary();
            TabManager.instance.ViewSummary();
        }
        else
        {
            if(roundCost>money) InstantiatePopUp("Uh oh", "Insufficient balance", "Hint: try to remove some dishes!");
            if(roundCost == 0) InstantiatePopUp("Uh oh", "Are you trying to starve our patient?", "Hint: add some dishes for the patient!");
        }
    }

    public void ResetRound()
    {
        selectedDishes.Clear();
        roundCost = 0;
        UpdateCost();
        roundWholeGrainServing = 0;
        roundProteinServing = 0;
        roundVeggieServing = 0;
        UpdatePortionUI();
        ResetDishWindow();
        ResetPastMeals();
        ResetStars();
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
        stars+= count;
        starText.GetComponent<TextMeshProUGUI>().text = stars.ToString();
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
        //UpdateDishBtns();
    }

    public void ResetNutrientBar()
    {
        wholeGrainsBar.GetComponent<ProgressBar>().current = 0;
        proteinBar.GetComponent<ProgressBar>().current = 0;
        fruitVeggieBar.GetComponent<ProgressBar>().current = 0;
    }

    public void NewPatient()
    {
        ResetRound();
        AnimatePopUpText(moneyText, $"+$15.00", Color.yellow);
        money += 15;
        UpdateMoney();
        TabManager.instance.ViewHelp();
        //instantiate new patient and update patient info tab
        currentPatient = PatientFactory.instance.CreateNewStudent();
        Debug.Log("Required calories :" + currentPatient.calorie);
        Debug.Log("current calories :" + currentPatient.GetCurrentCalories());
        currentPatient.allergies = Patient.Allergies.ShellFish;
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
                $"Fruits n Veggie Serving: {dish.veggieServings}\n\n";
        tooltiptext += $"Contains: ";
        if (dish.ingredients.Count() == 1)
        {
            tooltiptext += dish.ingredients[0].ingredientName.ToString();
        }
        else
        {
            foreach (Ingredient ingredient in dish.ingredients)
            {
                if (ingredient.ingredientName.Equals(dish.ingredients[dish.ingredients.Count() - 1].ingredientName))
                {
                    tooltiptext += ingredient.ingredientName.ToString();
                }
                else
                {
                    tooltiptext += ingredient.ingredientName.ToString() + ", ";
                }

            }
        }

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
        Vector3 scaleAmount = new Vector3(0f, 0f, 0f);
        LeanTween.scale(star1, scaleAmount, 2f);
        LeanTween.scale(star2, scaleAmount, 2f);
        LeanTween.scale(star3, scaleAmount, 2f);
    }
}