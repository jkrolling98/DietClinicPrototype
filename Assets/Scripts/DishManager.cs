using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class DishManager : MonoBehaviour
{
    private string filePath = "dishes.json";
    public List<DishData> dishes = new List<DishData>();

    void Start()
    {
        // Run once
        Initialise();
        // Load the saved data
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
        }
    }

    public void Initialise()
    {
        DishData banana = new DishData("Banana", "A curved, yellow fruit with a thick skin and soft sweet flesh.", 0.6, 0, 0, 1, 93, 1.08, 0.15, 0.05, 2, 21.91, 0, 16.91);
        dishes.Add(banana);
        Save();
    }

    [System.Serializable]
    private class DishWrapper
    {
        public List<DishData> dishes;
    }
}
