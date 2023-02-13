using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient
{
    public string ingredientName;
    public allergen allergenOf;
    public bool isVegan;
    public bool isVegetarian;
    
    public enum allergen
    {
        NIL,
        Milk,
        Eggs,
        ShellFish,
        Peanuts
    }

    public Ingredient(string ingredientName, allergen allergenOf = allergen.NIL, bool isVegan = true, bool isVegetarian = true)
    {
        this.ingredientName = ingredientName;
        this.allergenOf = allergenOf;
        this.isVegan = isVegan;
        this.isVegetarian = isVegetarian;
    }
}
