using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dish : MonoBehaviour
{
    public string dishName;
    public Sprite image;
    [TextArea]
    public string description;
    public double cost;
    public int wholeGrainServings;
    public int proteinServings;
    public int veggieServings;
    public double calories; //in kcal
    public double protein; // in g
    public double totalFats; //in g
    public double saturatedFats; // in g
    public double dietaryFibre; // in g
    public double carbohydrate; // in g
    public double cholesterol; //in mg
    public double sodium; //in mg
    public Ingredient[] ingredients;


    public void setDishData(DishData data)
    {
        this.dishName = data.dishName;
        this.image = Resources.Load<Sprite>("Sprite/" + dishName.Replace(" ",""));
        this.description = data.description;
        this.cost = data.cost;
        this.wholeGrainServings = data.wholeGrainServings;
        this.proteinServings = data.proteinServings;
        this.veggieServings = data.veggieServings;
        this.calories = data.calories;
        this.protein = data.protein;
        this.totalFats = data.totalFats;
        this.saturatedFats = data.saturatedFats;
        this.dietaryFibre = data.dietaryFibre;
        this.carbohydrate = data.carbohydrate;
        this.cholesterol = data.cholesterol;
        this.sodium = data.sodium;
        this.ingredients = data.ingredients;
    }

    public void setDishData(Dish data)
    {
        this.dishName = data.dishName;
        this.image = data.image;
        this.description = data.description;
        this.cost = data.cost;
        this.wholeGrainServings = data.wholeGrainServings;
        this.proteinServings = data.proteinServings;
        this.veggieServings = data.veggieServings;
        this.calories = data.calories;
        this.protein = data.protein;
        this.totalFats = data.totalFats;
        this.saturatedFats = data.saturatedFats;
        this.dietaryFibre = data.dietaryFibre;
        this.carbohydrate = data.carbohydrate;
        this.cholesterol = data.cholesterol;
        this.sodium = data.sodium;
        this.ingredients = data.ingredients;
    }

    public bool IsVegan()
    {
        foreach(Ingredient ingredient in ingredients)
        {
            if (ingredient.isVegan == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsVegetarian()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.isVegetarian == false)
            {
                return false;
            }
        }
        return true;
    }

    public List<string> GetAllergenList()
    {
        List<string> allergenList = new List<string>();
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.allergenOf != Ingredient.allergen.NIL)
            {
                if (!allergenList.Contains(ingredient.allergenOf.ToString()))
                {
                    allergenList.Add(ingredient.allergenOf.ToString());
                }
            }
        }
        return allergenList;
    }

}