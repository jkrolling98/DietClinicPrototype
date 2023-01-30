using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dish : MonoBehaviour
{
    public string dishName;
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

    public Dish(DishData data)
    {
        data.dishName = dishName;
        data.description = description;
        data.cost = cost;
        data.wholeGrainServings = wholeGrainServings;
        data.proteinServings = proteinServings;
        data.veggieServings = veggieServings;
        data.calories = calories;
        data.protein = protein;
        data.totalFats = totalFats;
        data.saturatedFats = saturatedFats;
        data.dietaryFibre = dietaryFibre;
        data.carbohydrate = carbohydrate;
        data.cholesterol = cholesterol;
        data.sodium = sodium;
    }
}