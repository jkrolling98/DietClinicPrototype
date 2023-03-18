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
        Ingredient[] gardenSaladIngredients = new Ingredient[] { new Ingredient("Lettuce"), new Ingredient("Tomatoes"), new Ingredient("Spinach") };
        DishData gardenSalad = new DishData("Garden Salad", "Salad with 3 vegetable toppings, no salad dressing.", 4, 0, 0, 2, 53.19, 4.05, 1.37, 0.25, 4.3, 7.59, 0, 326.37, gardenSaladIngredients);
        Ingredient[] hokkienMeeIngredients = new Ingredient[] { new Ingredient("Yellow Noodle"), new Ingredient("Rice Vermicelli"), new Ingredient("Prawn",Ingredient.allergen.ShellFish,false,false), new Ingredient("Cuttle Fish", Ingredient.allergen.ShellFish, false, false) };
        DishData hokkienMee = new DishData("Hokkien Mee", "Fried mixture of yellow noodle and thick rice vermicelli, with added prawn and cuttlefish.", 5.5, 2, 1, 0, 521.56, 18.12, 19.01, 7.34, 4.42, 69.39, 132.6, 1423.24, hokkienMeeIngredients);
        Ingredient[] seaweedChickenIngredients = new Ingredient[] { new Ingredient("Chicken",Ingredient.allergen.NIL,false,false), new Ingredient("Seaweed") };
        DishData seaweedChicken = new DishData("Seaweed Chicken", "Sliced chicken coated with seasoned flourand wrapped with seaweed, deep fried. Comes in 5.", 2, 0, 2, 0, 214.2, 15.95, 12.15, 3.85, 2.05, 10.45, 31.5, 652.5, seaweedChickenIngredients);
        Ingredient[] wholeGrainCerealIngredients = new Ingredient[] { new Ingredient("WholeGrain Cereal"), new Ingredient("Milk", Ingredient.allergen.Milk, false, true) };
        DishData wholeGrainCereal = new DishData("WholeGrain Cereal", "Wholegrain breakfast cereal.", 2.5, 2, 0, 0, 95.4, 2.25, 0.93, 0.2, 1.74, 19.47, 0, 171, wholeGrainCerealIngredients);
        Ingredient[] tofuIngredients = new Ingredient[] { new Ingredient("tofu")};
        DishData tofu = new DishData("Tofu", "A creamy, high-protein, low-fat soy product also known as bean curd.", 1.5, 0, 2, 0, 123.15, 12.31, 5.62, 0.91, 0.61, 5.78, 0, 10.94, tofuIngredients);
        Ingredient[] roastedChickenBreastSaladIngredients = new Ingredient[] { new Ingredient("Chicken Breast",Ingredient.allergen.NIL, false, false), new Ingredient("Lettuce"), new Ingredient("Tomatoes"), new Ingredient("Onions"), new Ingredient("Green Peppers") };
        DishData roastedChickenBreastSalad = new DishData("Roasted Chicken Breast Salad", "A salad that features roasted chicken breast with fresh greens.", 4, 0, 1, 2, 136.04, 17.9, 2.61, 0.72, 4.01, 10.99, 50.12, 282.82, roastedChickenBreastSaladIngredients);
        Ingredient[] appleIngredients = new Ingredient[] { new Ingredient("Apple") };
        DishData apple = new DishData("Apple", "a round or oval-shaped fruit with a red, green, or yellow skin and a firm, juicy flesh inside.", 0.6, 0, 0, 1, 79.21, 0.41, 0.28, 0, 3.17, 16.84, 0, 1.38, appleIngredients);
        Ingredient[] thunderTeaRiceIngredients = new Ingredient[] { new Ingredient("Brown rice"), new Ingredient("tea"), new Ingredient("vegetables"), new Ingredient("beancurd") };
        DishData thunderTeaRice = new DishData("Thunder Tea Rice", "a vegetarian variation of thunder tea rice.", 5.5, 2, 1, 2, 642, 26.54, 33.94, 10.19, 15.41, 57.78, 0, 1335.36, thunderTeaRiceIngredients);
        dishes.Add(chickenBriyani);
        dishes.Add(popiah);
        dishes.Add(banana);
        dishes.Add(chickenRice);
        dishes.Add(gardenSalad);
        dishes.Add(hokkienMee);
        dishes.Add(seaweedChicken);
        dishes.Add(wholeGrainCereal);
        dishes.Add(muttonBriyani);
        dishes.Add(tofu);
        dishes.Add(roastedChickenBreastSalad);
        dishes.Add(apple);
        dishes.Add(thunderTeaRice);
        Save();
    }

    public static List<Dish> GetDishes()
    {
        //Debug.Log("getting dishes");
        List<Dish> dishList = new List<Dish>();
        foreach(DishData dish in dishes)
        {
            Dish dishclass = new Dish(); 
            dishclass.setDishData(dish);
            dishList.Add(dishclass);
            //Debug.Log("adding dish: "+dishclass.dishName);
        }
        return dishList;
    }

    [Serializable]
    private class DishWrapper
    {
        public List<DishData> dishes;
    }

}
