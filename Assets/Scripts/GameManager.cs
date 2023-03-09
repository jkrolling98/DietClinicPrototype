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
    public GameObject roundSummaryPopUp;
    public GameObject pastMealWindow;
    public GameObject pastMealTemplate;
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
    public GameObject emotes;
    public GameObject moneyCounter;
    public GameObject costText;
    public GameObject calorieText;
    public GameObject caloriesBar;
    public GameObject starText;
    public GameObject dayCounter;
    public GameObject customerCounter;
    public GameObject customerIcon;
    public GameObject paymentUI;

    public static List<Dish> allDishes;

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
    public int day = 0;
    public int customerCount;
    public int roundGoal;
    public double payment = 0;

    public int totalStars = 0;
    public int totalCustomerCount = 0;

    private Patient currentPatient;
    public List<Dish> selectedDishes;

    private float timerDuration = 180f;
    public TextMeshProUGUI timerText;
    public GameObject timerBar;
    private float currentTime;
    public static bool isRunning = false;

    public Sprite servingReference;
    public Sprite profitSprite;
    public Sprite lossSprite;
    public Sprite goldMedal;
    public Sprite silverMedal;
    public Sprite bronzeMedal;

    //public static GameManager instance;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(this);
    //        return;
    //    }
    //    instance = this;
    //}

    void Start()
    {
        money = 0;
        UpdateMoney();
        allDishes = DishManager.GetDishes();
        SetDishWindow();
        StartCoroutine(StartDay());
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            timerBar.GetComponent<ProgressBar>().current = (int)currentTime/1;
            if (currentTime <= 0)
            {
                isRunning = false;
                OnTimerFinished();
            }

            if(customerCount == 0)
            {
                isRunning=false;
                StartCoroutine(OnDayComplete());
            }
        }
    }

    public void OnTimerFinished()
    {
        Debug.Log("times up! gameover...");
        StartCoroutine(OnDayComplete());
    }

    public IEnumerator OnDayComplete()
    {
        ResetDishWindow();
        //check if in loss
        if (stars >= roundGoal)
        {
            string daySummary = $"Well done! Day {day} complete!\n" +
            $"You have gathered a total of {stars} stars!";
            string footer = $"Day profits: ${(money + payment).ToString("0.00")}";
            yield return StartCoroutine(InstantiatePopUp("Day Complete!", daySummary, footer, goldMedal));
            day++;
            StartCoroutine(StartDay());
        }
        else
        {
            yield return StartCoroutine(InstantiatePopUp("Game Over", $"Customers were not impressed...\nYou were {roundGoal - stars} stars short of meeting the requirement."));
            stars = 0;
            //UpdateStars();
            day = 0;

            StartCoroutine(StartDay());
        }
    }

    public IEnumerator StartDay()
    {
        UpdateStars();
        if (day == 0)
        {
            string headerText = "Welcome to Diet Clinic";
            string bodyText = "Help our patient find the ideal meal combinations and aim to get as many 3* reviews as possible!";
            string footerText = "Hint: Follow the quarter quarter half meal plan!";
            yield return StartCoroutine(InstantiatePopUp(headerText, bodyText, footerText, servingReference));
            yield return StartCoroutine(PlayTutorial(1));
        }
        day++;
        UpdateDay();
        customerCount = 5;
        roundGoal = 8;
        UpdateCustomerCounter();
        currentTime = timerDuration;
        NewPatient();
        yield return StartCoroutine(InstantiatePopUp($"Day {day} Start", $"Serve {customerCount} customers and achieve {roundGoal} stars within 3 mins to progress!"));
    }

    public IEnumerator PlayTutorial(int num)
    {
        isRunning = false;
        GameObject tutorial = Resources.Load<GameObject>($"Tutorials/Tutorial_{num.ToString()}");
        GameObject popUp = Instantiate(tutorial, canvas.transform.position, Quaternion.identity, canvas.transform);
        yield return StartCoroutine(popUp.GetComponent<TutorialManager>().WaitForClose());
        isRunning = true;
    }

    public void UpdateCustomerCounter()
    {
        for (int i = customerCounter.transform.childCount - 1; i >= 1; i--)
        {
            Destroy(customerCounter.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < customerCount; i++)
        {
            GameObject customer = Instantiate(customerIcon, customerCounter.transform);
            customer.SetActive(true);
        }
    }

    public void NextDay()
    {
        totalStars += stars;
        totalCustomerCount += customerCount;
        stars = 0;
        UpdateStars();
        customerCount = 0;
        StartCoroutine(StartDay());
    }

    public IEnumerator InstantiatePopUp(string headerText, string bodyText, string footerText = null, Sprite contentImage = null, Sprite hintImage = null)
    {
        isRunning = false;
        GameObject popUp = Instantiate(popUpWindow, canvas.transform.position, Quaternion.identity, canvas.transform);
        TextMeshProUGUI header = popUp.transform.Find("PopUpWindow").Find("Header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI body = popUp.transform.Find("PopUpWindow").Find("Body").Find("BodyText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI footer = popUp.transform.Find("PopUpWindow").Find("Footer").Find("FooterText").GetComponent<TextMeshProUGUI>();
        header.text = headerText;
        body.text = bodyText;
        footer.text = footerText;
        popUp.transform.Find("PopUpWindow").Find("Footer").gameObject.SetActive(!string.IsNullOrEmpty(footerText));
        if (contentImage != null)
        {
            Image bodyImage = popUp.transform.Find("PopUpWindow").Find("Body").Find("BodyImage").GetComponent<Image>();
            bodyImage.sprite = contentImage;
            popUp.transform.Find("PopUpWindow").Find("Body").Find("BodyImage").gameObject.SetActive(true);
        }
        if (hintImage != null)
        {
            Image hint = popUp.transform.Find("PopUpWindow").Find("HelpWindow").Find("HelpImage").GetComponent<Image>();
            hint.sprite = hintImage;
            popUp.transform.Find("PopUpWindow").Find("Header").Find("HintBtn").gameObject.SetActive(true);
        }
        yield return StartCoroutine(popUp.GetComponent<PopUpManager>().WaitForClose());
        isRunning = true;
    }

    public IEnumerator InstantiateRoundSummary(string bodyText, string footerText, int count)
    {
        isRunning = false;
        GameObject popUp = Instantiate(roundSummaryPopUp, canvas.transform.position, Quaternion.identity, canvas.transform);
        TextMeshProUGUI body = popUp.transform.Find("RoundSummaryWindow").Find("Body").Find("BodyText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI footer = popUp.transform.Find("RoundSummaryWindow").Find("Body").Find("FooterText").GetComponent<TextMeshProUGUI>();
        body.text = bodyText;
        footer.text = footerText;
        yield return StartCoroutine(popUp.GetComponent<PopUpManager>().WaitForClose(count));
        NewPatient();
        isRunning = true;
    }

    public void UpdateMoney()
    {
        moneyCounter.GetComponent<Image>().sprite = money < 0 ? lossSprite : profitSprite;
        moneyCounter.GetComponentInChildren<TextMeshProUGUI>().text = $"${money.ToString("0.00")}";
    }

    public void UpdateCost()
    {
        costText.GetComponent<TextMeshProUGUI>().text = $"${roundCost.ToString("0.00")}";
    }

    public void UpdateCalorie()
    {
        calorieText.GetComponent<TextMeshProUGUI>().text = $"{roundCalorie.ToString()} kcal";
        caloriesBar.GetComponent<ProgressBar>().current = roundCalorie;
    }

    public void UpdateStars()
    {
        starText.GetComponent<TextMeshProUGUI>().text = stars.ToString()+"/"+roundGoal.ToString();
    }

    public void UpdateDay()
    {
        dayCounter.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + day.ToString();
        dayCounter.SetActive(true);
    }

    public void UpdatePayment()
    {
        paymentUI.SetActive(payment > 0);
        if(payment > 0)
        {
            paymentUI.GetComponent<HoverTip>().tipToShow = $"Click to collect payment! \n\nTotal payment amount: {payment}";
            //adding vibration effects
            LeanTween.scale(paymentUI, Vector3.one * 1.05f, 0.5f)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopCount(2)
            .setLoopPingPong();
        }
    }

    public void CollectPayment()
    {
        money += payment;
        UpdateMoney();
        AnimatePopUpText(moneyCounter.transform.Find("MoneyText").gameObject, $"+${payment.ToString("0.00")}", Color.yellow);
        payment = 0;
        UpdatePayment();
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
        double portionScore=0; //out of 5
        double calorieScore=0; //out of 5
        if(roundVeggieServing == 2*roundWholeGrainServing && roundVeggieServing == 2 * roundProteinServing)
        {
            portionScore = 5;
        }
        else if(roundVeggieServing >= roundWholeGrainServing && roundVeggieServing >= roundProteinServing)
        {
            portionScore +=3;
        }
        if(roundVeggieServing == 0 || roundProteinServing ==0 | roundWholeGrainServing == 0)
        {
            portionScore -= 1;
        }

        if(roundCalorie <= currentPatient.calorie + 100 && roundCalorie >= currentPatient.calorie - 100)
        {
            calorieScore = 5;
        }
        else if (roundCalorie <= currentPatient.calorie + 150 && roundCalorie >= currentPatient.calorie - 150)
        {
            calorieScore = 4;
        }
        else if (roundCalorie <= currentPatient.calorie + 200 && roundCalorie >= currentPatient.calorie - 200)
        {
            calorieScore = 3;
        }
        else if (roundCalorie <= currentPatient.calorie + 250 && roundCalorie >= currentPatient.calorie - 250)
        {
            calorieScore = 2;
        }
        else if (roundCalorie <= currentPatient.calorie + 300 && roundCalorie >= currentPatient.calorie - 300)
        {
            calorieScore = 1;
        }
        else calorieScore = 0;
        string summary = $"Grain consumed : {roundWholeGrainServing}\n" +
            $"Protein consumed : {roundProteinServing}\n" +
            $"Fruit consumed : {roundVeggieServing}\n\n" +
            $"Portion score : {portionScore}\n" +
            $"Calories score : {calorieScore}\n" +
            $"Money spent : ${roundCost.ToString("0.00")}\n";
        double OverallScore = portionScore + calorieScore;
        Debug.Log(OverallScore);
        Debug.Log("checking for repeats");
        int repeatCounts = 0;
        bool allergyTriggered = false;
        List<string> penaltyList = new List<string>();
        foreach (Dish dish in selectedDishes)
        {
            if (dish.dishName == currentPatient.meals[0].dishName || dish.dishName == currentPatient.meals[1].dishName)
            {
                Debug.Log($"Penalty! {dish.dishName} is repeated.");
                penaltyList.Add($"{dish.dishName} is repeated.");
                repeatCounts++;
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
                        allergyTriggered = true;
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
        // for each repeat, - 2 from overall score
        OverallScore -= repeatCounts * 2;
        if (allergyTriggered) OverallScore = 0;
        int starCount = 0;
        string footer;
        double tipAmount = 0;
        if (OverallScore >= 8)
        {
            starCount = 3;
            tipAmount = roundCost * 1.5;
            footer = $"For the awesome service, {currentPatient.patientName} has left a generous payment of {tipAmount}.";
            payment += tipAmount;
            UpdatePayment();
        }
        else if (OverallScore >= 6)
        {
            starCount = 2;
            tipAmount = roundCost * 1.2;
            footer = $"For the good service, {currentPatient.patientName} has left a payment of {tipAmount}.";
            payment += tipAmount;
            UpdatePayment();
        }
        else if (OverallScore >= 3)
        {
            starCount = 1;
            tipAmount = roundCost * 0.8;
            footer = $"For the poor service, {currentPatient.patientName} felt like he is being overcharged. Hence only paid {tipAmount}.";
            payment += tipAmount;
            UpdatePayment();
        }
        else
        {
            footer = $"For the poor service, {currentPatient.patientName} has left without paying.";
        }
        UpdateLevel(starCount);
        switch (starCount)
        {
            case 0:
                //StartCoroutine(UpdateSpeechBubble("That was the worst!"));
                StartCoroutine(UpdateEmotes("E34"));
                UpdatePatientSprite("upset");
                break;
            case 1:
                //StartCoroutine(UpdateSpeechBubble("Could be better..."));
                StartCoroutine(UpdateEmotes("E25"));
                UpdatePatientSprite("neutral");
                break;
            case 2:
                //StartCoroutine(UpdateSpeechBubble("Not too shabby!"));
                StartCoroutine(UpdateEmotes("E1"));
                UpdatePatientSprite("happy");
                break;
            case 3:
                //StartCoroutine(UpdateSpeechBubble("That was awesome!"));
                StartCoroutine(UpdateEmotes("E11"));
                UpdatePatientSprite("elated");
                break;
            default: break;
        }
        StartCoroutine(InstantiateRoundSummary(summary, footer, starCount));
    }

    public void Serve()
    {
        if(roundCost!=0)
        {
            isRunning = false;
            money -= roundCost;
            UpdateMoney();
            //change to current - initial / max
            UpdateSummary();
            //update customer counter
            GameObject customerIcon = customerCounter.transform.GetChild(customerCount).gameObject;
            customerIcon.GetComponent<Image>().color = Color.black;
            customerCount--;
        }
        else
        {
            //if(roundCost>money) StartCoroutine(InstantiatePopUp("Uh oh", "Insufficient balance", "Hint: try to remove some dishes!"));
            if(selectedDishes.Count==0) StartCoroutine(InstantiatePopUp("Uh oh", "Are you trying to starve our patient?", "Hint: add some dishes for the patient!"));
        }
    }

    public void ResetRound()
    {
        selectedDishes.Clear();
        roundCost = 0;
        UpdateCost();
        roundCalorie = 0;
        UpdateCalorie();
        roundWholeGrainServing = 0;
        roundProteinServing = 0;
        roundVeggieServing = 0;
        UpdatePortionUI();
        ResetDishWindow();
        ResetPastMeals();
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
        UpdateStars();
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
        //AnimatePopUpText(moneyCounter.transform.Find("MoneyText").gameObject, $"+$15.00", Color.yellow);
        //money += 15;
        UpdateMoney();
        TabManager.instance.ViewHelp();
        //instantiate new patient and update patient info tab
        currentPatient = PatientFactory.instance.CreateNewStudent();
        Debug.Log("Required calories :" + currentPatient.calorie);
        caloriesBar.GetComponent<ProgressBar>().maximum = currentPatient.calorie;
        Debug.Log("current calories :" + currentPatient.GetCurrentCalories());
        roundCalorie = currentPatient.GetCurrentCalories();
        UpdateCalorie();
        caloriesBar.GetComponent<ProgressBar>().current = roundCalorie;
        SetPatientSprite();
        UpdatePatientInfo(currentPatient);
        //currentTime = timerDuration;
        StartCoroutine(UpdateSpeechBubble("Can't wait to eat!"));
        isRunning = true;
    }

    public void SetPatientSprite()
    {
        if(currentPatient.gender == Patient.Gender.Male)
        {
            patient.GetComponent<Image>().sprite = currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Male_01/" + "neutral") : Resources.Load<Sprite>($"Patients/Male_02/" + "neutral");
        }
        else
        {
            patient.GetComponent<Image>().sprite = currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Female_01/" + "neutral") : Resources.Load<Sprite>($"Patients/Female_02/" + "neutral");
        }
    }

    public void UpdatePatientSprite(string emotion)
    {
        if (currentPatient.gender == Patient.Gender.Male)
        {
            patient.GetComponent<Image>().sprite = currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Male_01/" + emotion) : Resources.Load<Sprite>($"Patients/Male_02/" + emotion);
        }
        else
        {
            patient.GetComponent<Image>().sprite = currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Female_01/" + emotion) : Resources.Load<Sprite>($"Patients/Female_02/" + emotion);
        }
    }

    public IEnumerator UpdateSpeechBubble(string text, float duration = 5f)
    {
        emotes.SetActive(false);
        speechBubble.transform.Find("SpeechText").GetComponent<TextMeshProUGUI>().text = text;
        speechBubble.SetActive(true);
        speechBubble.transform.Find("SpeechText").gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        speechBubble.SetActive(false);
    }

    public IEnumerator UpdateEmotes(string emoteName, float duration = 5f)
    {
        emotes.GetComponent<Image>().sprite = Resources.Load<Sprite>("Emotes/" + emoteName);
        emotes.SetActive(true);
        speechBubble.transform.Find("SpeechText").gameObject.SetActive(false);
        speechBubble.SetActive(true);
        yield return new WaitForSeconds(duration);
        speechBubble.SetActive(false);
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
}