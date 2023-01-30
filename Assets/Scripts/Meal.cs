using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meal", menuName = "ScriptableObjects/Meal", order = 1)]
public class Meal : ScriptableObject
{
    public string dishName;
    public Sprite image;
    [TextArea]
    public string description;
    public float cost;
    public int wholeGrainServings;
    public int proteinServings;
    public int veggieServings;
    public float calories; //in kcal
    public float protein; // in g
    public float totalFats; //in g
    public float saturatedFats; // in g
    public float dietaryFibre; // in g
    public float carbohydrate; // in g
    public float cholesterol; //in mg
    public float sodium; //in mg
}

