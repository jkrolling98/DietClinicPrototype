using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DishManager : MonoBehaviour
{
    private string filePath = "dishes.json";
    public static List<DishData> dishes = new List<DishData>();

    private void Awake()
    {
        Load();
    }

    public void Save()
    {
        DishWrapper dishWrapper = new DishWrapper { dishes = dishes };
        // Serialize the data to a JSON string
        string json = JsonUtility.ToJson(dishWrapper, true);
        // Save the JSON string to a file
        System.IO.File.WriteAllText(filePath, json);
    }

    public void Load()
    {
        // Check if the save file exists
        if (System.IO.File.Exists(filePath))
        {
            // Read the JSON string from the file
            string loadData = System.IO.File.ReadAllText(filePath);
            // Deserialize the JSON string to a list of Dish objects
            DishWrapper dishWrapper = JsonUtility.FromJson<DishWrapper>(loadData);
            dishes = dishWrapper.dishes;
            //foreach(DishData dish in dishes)
            //{
            //    Debug.Log(dish.dishName + " \n");
            //    foreach(Ingredient ingredient in dish.ingredients)
            //    {
            //        Debug.Log(ingredient.ingredientName);
            //    }
            //}
        }
        else
        {
            // If the save file does not exist, create a new list
            dishes = new List<DishData>();
            Initialise();
        }
    }

    public void Initialise()
    {
        Ingredient[] bananaIngredients = new Ingredient[] { new Ingredient("Banana") };
        DishData banana = new DishData("Banana", "A curved, yellow fruit with a thick skin and soft sweet flesh.", 0.6, 0, 0, 1, 93, 1.08, 0.15, 0.05, 2, 21.91, 0, 16.91, bananaIngredients);
        Ingredient[] chickenBriyaniIngredients = new Ingredient[] { new Ingredient("Rice"), new Ingredient("Chicken", Ingredient.allergen.NIL, false, false), new Ingredient("Ghee", Ingredient.allergen.Milk)};
        DishData chickenBriyani = new DishData("Chicken Briyani", "Rice cooked with ghee and spices, served with spicy chicken", 6, 2, 1, 0, 754.6, 33.6, 30, 12.8, 7.2, 88, 136, 1428, chickenBriyaniIngredients);
        Ingredient[] chickenRiceIngredients = new Ingredient[] { new Ingredient("Rice"), new Ingredient("Chicken",Ingredient.allergen.NIL,false,false), new Ingredient("Cucumber"), new Ingredient("Chilli") };
        DishData chickenRice = new DishData("Chicken Rice", "A dish of poached chicken and seasoned rice, served with chilli sauce and usually with cucumber garnishes.", 4.5, 2, 1, 0, 557.7, 28.05, 13.86, 4.95, 3.3, 80.19, 36.63, 697.95, chickenRiceIngredients);
        Ingredient[] muttonBriyaniIngredients = new Ingredient[] { new Ingredient("Rice"), new Ingredient("Mutton", Ingredient.allergen.NIL, false, false), new Ingredient("Ghee", Ingredient.allergen.Milk) };
        DishData muttonBriyani = new DishData("Mutton Briyani", "Rice cooked with ghee and spices, served with spicy mutton", 7, 2, 1, 0, 751.95, 35.85, 24.75, 12.12, 8.08, 96.46, 95.95, 1858.4, muttonBriyaniIngredients);
        Ingredient[] popiahIngredients = new Ingredient[] { new Ingredient("Radish"), new Ingredient("Egg",Ingredient.allergen.Eggs), new Ingredient("Chinese Sausage",Ingredient.allergen.NIL,false,false), new Ingredient("Flour") };
        DishData popiah = new DishData("Popiah", "Radish, eggs, chinese dried sausage and sweet black sauce wrapped in a flour-based skin", 3.5, 1, 1, 1, 187.57, 7.55, 11.19, 3.64, 4.06, 14.27, 44.76, 675.67, popiahIngredients);
        dishes.Add(banana);
        dishes.Add(chickenBriyani);
        dishes.Add(chickenRice);
        dishes.Add(muttonBriyani);
        dishes.Add(popiah);
        Save();
    }

    public static List<Dish> GetDishes()
    {
        Debug.Log("getting dishes");
        Debug.Log(dishes[0].dishName);
        List<Dish> dishList = new List<Dish>();
        foreach(DishData dish in dishes)
        {
            Debug.Log("here");
            Dish dishclass = new Dish(); 
            dishclass.setDishData(dish);
            dishList.Add(dishclass);
            Debug.Log("adding dish: "+dishclass.dishName);
        }

        return dishList;
    }

    [Serializable]
    private class DishWrapper
    {
        public List<DishData> dishes;
    }

    [Serializable]
    private class IngredientWrapper
    {
        public List<Ingredient> ingredients;
    }
}
