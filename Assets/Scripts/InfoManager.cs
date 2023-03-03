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
    public GameObject ingredientContentWindow;
    public GameObject ingredientTemplate;
    public Sprite tick;
    public Sprite cross;
    public GameObject detailsWindow;
    public GameObject ingredientsWindow;

    public GameObject dishIcon;
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
        //set info panel
        //icon
        dishIcon.GetComponent<Image>().sprite = currentDish.image;
        //headers
        dishName.text = currentDish.dishName;
        wholeGrain.text = currentDish.wholeGrainServings.ToString();
        protein.text = currentDish.proteinServings.ToString();
        veggie.text = currentDish.veggieServings.ToString();
        //body
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
        //set ingredient panel
        ResetIngredients();
        foreach (Ingredient ingredient in currentDish.ingredients)
        {
            GameObject ingredientRow = Instantiate(ingredientTemplate, ingredientContentWindow.transform);
            ingredientRow.transform.Find("IngredientName").GetComponent<TextMeshProUGUI>().text = ingredient.ingredientName;
            ingredientRow.transform.Find("Allergen").GetComponent<TextMeshProUGUI>().text = ingredient.allergenOf.ToString();
            ingredientRow.transform.Find("Vegan").GetComponent<Image>().sprite = ingredient.isVegan? tick : cross;
            ingredientRow.transform.Find("Vegetarian").GetComponent<Image>().sprite = ingredient.isVegetarian ? tick : cross;
            ingredientRow.SetActive(true);
        }
    }

    public void DisplayDishDetails()
    {
        detailsWindow.SetActive(true);
        ingredientsWindow.SetActive(false);
    }

    public void DisplayDishIngredients()
    {
        ingredientsWindow.SetActive(true);
        detailsWindow.SetActive(false);
    }

    public void ResetIngredients()
    {
        if (ingredientContentWindow.transform.childCount > 1)
        {
            for (int i = ingredientContentWindow.transform.childCount - 1; i >= 1; i--)
            {
                //Debug.Log(pastMealWindow.transform.GetChild(i));
                GameObject.Destroy(ingredientContentWindow.transform.GetChild(i).gameObject);
            }
        }
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
