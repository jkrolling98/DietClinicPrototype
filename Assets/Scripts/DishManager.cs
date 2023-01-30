using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DishManager : MonoBehaviour
{
    private string filePath = "dishes.json";
    public static List<DishData> dishes = new List<DishData>();

    public static DishManager instance;

    private void Awake()
    {
        Load();
        Debug.Log("loaded");
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Start()
    {
        // Run once
        //Initialise();
        //Debug.Log("Initialised");
        // Load the saved data
        
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
            Debug.Log("loaddata: " + loadData);
            // Deserialize the JSON string to a list of Dish objects
            DishWrapper dishWrapper = JsonUtility.FromJson<DishWrapper>(loadData);
            Debug.Log("dishwrapper: "+ JsonUtility.ToJson(dishWrapper, true));
            Debug.Log(dishWrapper.dishes.Count);
            dishes = dishWrapper.dishes;
            string output = "";
            foreach(DishData dish in dishes)
            {
                output += dish.dishName + " \n";
            }
            Debug.Log("dishes: "+output);
            Debug.Log(dishes);
            Debug.Log(dishes.Count);
        }
        else
        {
            // If the save file does not exist, create a new list
            dishes = new List<DishData>();
        }
    }

    public void Initialise()
    {
        DishData banana = new DishData("Banana", "A curved, yellow fruit with a thick skin and soft sweet flesh.", 0.6, 0, 0, 1, 93, 1.08, 0.15, 0.05, 2, 21.91, 0, 16.91);
        DishData chickenBriyani = new DishData("Chicken Briyani", "Rice cooked with ghee and spices, served with spicy chicken", 6, 2, 1, 0, 754.6, 33.6, 30, 12.8, 7.2, 88, 136, 1428);
        DishData chickenRice = new DishData("Chicken Rice", "A dish of poached chicken and seasoned rice, served with chilli sauce and usually with cucumber garnishes.", 4.5, 2, 1, 0, 557.7, 28.05, 13.86, 4.95, 3.3, 80.19, 36.63, 697.95);
        DishData muttonBriyani = new DishData("Mutton Briyani", "Rice cooked with ghee and spices, served with spicy mutton", 7, 2, 1, 0, 751.95, 35.85, 24.75, 12.12, 8.08, 96.46, 95.95, 1858.4);
        DishData popiah = new DishData("Popiah", "Radish, eggs, chinese dried sausage and sweet black sauce wrapped in a flour-based skin", 3.5, 1, 1, 1, 187.57, 7.55, 11.19, 3.64, 4.06, 14.27, 44.76, 675.67);
        dishes.Add(banana);
        dishes.Add(chickenBriyani);
        dishes.Add(chickenRice);
        dishes.Add(muttonBriyani);
        dishes.Add(popiah);
        Save();
    }

    public List<Dish> GetDishes()
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

    [System.Serializable]
    private class DishWrapper
    {
        public List<DishData> dishes;
    }
}
