using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientFactory : MonoBehaviour
{

    public static PatientFactory instance;
    public List<Dish> allDishes;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        allDishes = DishManager.GetDishes();
    }

    private static string[] maleNames = { "John", "Ken", "Mike", "Peter", "Bob", "Sam" };
    private static string[] femaleNames = { "Michelle", "Jane", "Joanna", "Mary", "Barbara", "Sue" };

    public string RandomMaleName()
    {
        int index = Random.Range(0, maleNames.Length);
        return maleNames[index];
    }

    public string RandomFemaleName()
    {
        int index = Random.Range(0, femaleNames.Length);
        return femaleNames[index];
    }

    public Dish[] GenerateRandomMeals(int mealCount)
    {
        List<Dish> result = new List<Dish>();
        HashSet<int> indexes = new HashSet<int>();
        while (indexes.Count < mealCount)
        {
            indexes.Add(Random.Range(0, allDishes.Count));
        }
        foreach(int index in indexes)
        {
            result.Add(allDishes[index]);
        }
        return result.ToArray();
    }

    public Dish[] GenerateRandomMeals(int mealCount, Patient.FoodPref pref, Patient.Allergies allergy)
    {
        List<Dish> result = new List<Dish>();
        HashSet<int> indexes = new HashSet<int>();
        if(pref == Patient.FoodPref.NIL)
        {
            while (indexes.Count < mealCount)
            {
                int randIndex = Random.Range(0, allDishes.Count);
                if (!allDishes[randIndex].GetAllergenList().Contains(allergy.ToString()))
                {
                    indexes.Add(randIndex);
                }
            }
            foreach (int index in indexes)
            {
                result.Add(allDishes[index]);
            }
        }
        else if (pref == Patient.FoodPref.Vegan)
        {
            while (indexes.Count < mealCount)
            {
                int randIndex = Random.Range(0, allDishes.Count);
                Dish randDish = allDishes[randIndex];
                if (!randDish.GetAllergenList().Contains(allergy.ToString()) && randDish.IsVegan())
                {
                    indexes.Add(randIndex);
                }
            }
            foreach (int index in indexes)
            {
                result.Add(allDishes[index]);
            }
        }
        else if (pref == Patient.FoodPref.Vegetarian)
        {
            while (indexes.Count < mealCount)
            {
                int randIndex = Random.Range(0, allDishes.Count);
                Dish randDish = allDishes[randIndex];
                if (!randDish.GetAllergenList().Contains(allergy.ToString()) && randDish.IsVegetarian())
                {
                    indexes.Add(randIndex);
                }
            }
            foreach (int index in indexes)
            {
                result.Add(allDishes[index]);
            }
        }
        return result.ToArray();
    }

    public Patient CreateNewStudent(int day)
    {
        if (day <= 2)
        {
            if (Random.Range(0, 2) == 0)
            {
                string name = RandomMaleName();
                int age = Random.Range(11, 20);
                int weight = Random.Range(45, 100);
                int height = Random.Range(140, 180);
                Patient.FoodPref pref = Patient.FoodPref.NIL; //to be changed
                Patient.Allergies allergies = Patient.Allergies.NIL; //to be changed
                Patient.ActivityLevel activityLevel = (Patient.ActivityLevel)Random.Range(1, Enum.GetValues(typeof(Patient.ActivityLevel)).Length);

                //generate meals
                Dish[] meals = GenerateRandomMeals(2);

                return new Patient(name, age, weight, height, Patient.Gender.Male, Patient.Occupation.Student, pref, allergies, activityLevel, meals);
            }
            else
            {
                string name = RandomFemaleName();
                int age = Random.Range(11, 20);
                int weight = Random.Range(40, 80);
                int height = Random.Range(130, 180);
                Patient.FoodPref pref = Patient.FoodPref.NIL; //to be changed
                Patient.Allergies allergies = Patient.Allergies.NIL; //to be changed
                Patient.ActivityLevel activityLevel = (Patient.ActivityLevel)Random.Range(1, Enum.GetValues(typeof(Patient.ActivityLevel)).Length);

                //generate meals
                Dish[] meals = GenerateRandomMeals(2);

                return new Patient(name, age, weight, height, Patient.Gender.Female, Patient.Occupation.Student, pref, allergies, activityLevel, meals);
            }
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                string name = RandomMaleName();
                int age = Random.Range(11, 20);
                int weight = Random.Range(45, 100);
                int height = Random.Range(140, 180);
                Patient.FoodPref pref = (Patient.FoodPref)Random.Range(0, Enum.GetValues(typeof(Patient.FoodPref)).Length);
                Patient.Allergies allergies = (Patient.Allergies)Random.Range(0, Enum.GetValues(typeof(Patient.Allergies)).Length);
                Patient.ActivityLevel activityLevel = (Patient.ActivityLevel)Random.Range(1, Enum.GetValues(typeof(Patient.ActivityLevel)).Length);

                //generate meals
                Dish[] meals = GenerateRandomMeals(2, pref, allergies);

                return new Patient(name, age, weight, height, Patient.Gender.Male, Patient.Occupation.Student, pref, allergies, activityLevel, meals);
            }
            else
            {
                string name = RandomFemaleName();
                int age = Random.Range(11, 20);
                int weight = Random.Range(40, 80);
                int height = Random.Range(130, 180);
                Patient.FoodPref pref = (Random.Range(0, 3) != 0) ? Patient.FoodPref.NIL : (Patient.FoodPref)Random.Range(0, Enum.GetValues(typeof(Patient.FoodPref)).Length);
                Patient.Allergies allergies = (Random.Range(0, 3) != 0) ? Patient.Allergies.NIL : (Patient.Allergies)Random.Range(0, Enum.GetValues(typeof(Patient.Allergies)).Length);
                Patient.ActivityLevel activityLevel = (Patient.ActivityLevel)Random.Range(1, Enum.GetValues(typeof(Patient.ActivityLevel)).Length);

                //generate meals
                Dish[] meals = GenerateRandomMeals(2, pref, allergies);

                return new Patient(name, age, weight, height, Patient.Gender.Female, Patient.Occupation.Student, pref, allergies, activityLevel, meals);
            }
        }
    }
}
