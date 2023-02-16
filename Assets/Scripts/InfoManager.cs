using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public GameObject InfoWindow;
    public GameObject DishInfoContentWindow;
    public GameObject dishTemplate;
    private List<Dish> allDishes;

    public TextMeshProUGUI dishName;
    public TextMeshProUGUI wholeGrain;
    public TextMeshProUGUI protein;
    public TextMeshProUGUI veggie;
    public TextMeshProUGUI dishDescription;
    public TextMeshProUGUI dishDetails;

    void Start()
    {
        allDishes = DishManager.GetDishes();
        SetDishInfoWindow();
        //Auto select first child
        GameObject firstItem = DishInfoContentWindow.transform.GetChild(1).gameObject;
        firstItem.GetComponent<Toggle>().isOn = true;
        //DisplayDish(firstItem);
    }

    public void SetDishInfoWindow()
    {
        foreach (Dish dish in allDishes)
        {
            GameObject dishItem = Instantiate(dishTemplate, DishInfoContentWindow.transform);
            dishItem.name = dish.dishName;
            GameObject dishImage = dishItem.transform.Find("DishImage").gameObject;
            dishImage.GetComponent<Image>().sprite = dish.image;
            dishItem.SetActive(true);
            Dish dishComponent = dishItem.AddComponent<Dish>();
            dishComponent.setDishData(dish);
        }
    }

    public void DisplayDish(GameObject gameObject)
    {
        Dish currentDish = gameObject.GetComponent<Dish>();
        dishName.text = currentDish.dishName;
        wholeGrain.text = currentDish.wholeGrainServings.ToString();
        protein.text = currentDish.proteinServings.ToString();
        veggie.text = currentDish.veggieServings.ToString();
        dishDescription.text = currentDish.description;
        dishDetails.text = $"Cost ${currentDish.cost.ToString("0.00")}\n" +
                           $"Calories {currentDish.calories.ToString()}kcal\n" +
                           $"Protein {currentDish.protein.ToString()}g\n" +
                           $"TotalFat {currentDish.totalFats.ToString()}g\n" +
                           $"SaturatedFat {currentDish.saturatedFats.ToString()}g\n" +
                           $"DietaryFibre {currentDish.dietaryFibre.ToString()}g\n" +
                           $"Carbohydrate {currentDish.carbohydrate.ToString()}g\n" +
                           $"Cholesterol {currentDish.cholesterol.ToString()}mg\n" +
                           $"Sodium {currentDish.sodium.ToString()}mg";
    }

    public void DisplayWindow()
    {
        InfoWindow.SetActive(true);
    }

    public void CloseWindow()
    {
        InfoWindow.SetActive(false);
    }
}
