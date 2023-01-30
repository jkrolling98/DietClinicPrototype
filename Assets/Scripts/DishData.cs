using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DishData
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

    public DishData(string dishName, string description, double cost, int wholeGrainServings, int proteinServings, int veggieServings, double calories, double protein, double totalFats, double saturatedFats, double dietaryFibre, double carbohydrate, double cholesterol, double sodium)
    {
        this.dishName = dishName;
        this.description = description;
        this.cost = cost;
        this.wholeGrainServings = wholeGrainServings;
        this.proteinServings = proteinServings;
        this.veggieServings = veggieServings;
        this.calories = calories;
        this.protein = protein;
        this.totalFats = totalFats;
        this.saturatedFats = saturatedFats;
        this.dietaryFibre = dietaryFibre;
        this.carbohydrate = carbohydrate;
        this.cholesterol = cholesterol;
        this.sodium = sodium;
    }
}
