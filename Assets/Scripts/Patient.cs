using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{
    public string patientName;
    public int age;
    public int weight;
    public int height;
    public Gender gender;
    public Occupation occupation;
    public FoodPref preference;
    public Allergies allergies;
    public ActivityLevel activityLevel;
    public Dish[] meals;
    public int calorie;

    public enum Gender
    {
        Male,
        Female
    }

    public enum Occupation
    {
        Student,
        OfficeWorkers
    }

    public enum FoodPref
    {
        NIL,
        Vegan,
        Vegetarian,
        Paleo
    }

    public enum Allergies
    {
        NIL,
        Milk,
        Eggs,
        ShellFish,
        Peanuts
    }

    public enum ActivityLevel
    {
        NIL,
        Sedentary,
        Light,
        Moderate,
        VeryActive
    }

    public Patient(string patientName, int age, int weight, int height, Gender gender, Occupation occupation, FoodPref preference, Allergies allergies, ActivityLevel activityLevel, Dish[] meals)
    {
        this.patientName = patientName;
        this.age = age;
        this.weight = weight;
        this.height = height;
        this.gender = gender;
        this.occupation = occupation;
        this.preference = preference;
        this.allergies = allergies;
        this.activityLevel = activityLevel;
        this.meals = meals;
        int baseCalories = (gender == Gender.Male) ? (int)((10 * weight) + (6.25 * height) - (5 * age)+5) : (int)((10 * weight) + (6.25 * height) - (5 * age)-161);
        switch (activityLevel)
        {
            case ActivityLevel.Sedentary:
                this.calorie = (int)(baseCalories*1.2);
                break;
            case ActivityLevel.Light:
                this.calorie = (int)(baseCalories * 1.375);
                break;
            case ActivityLevel.Moderate:
                this.calorie = (int)(baseCalories * 1.55);
                break;
            case ActivityLevel.VeryActive:
                this.calorie = (int)(baseCalories * 1.725);
                break;
            default:
                this.calorie = baseCalories;
                break;
        }
    }

    public Patient(string name, int age, int weight, int height, Gender gender, Occupation occupation, FoodPref pref, Allergies allergies, Dish[] meals)
    {
        this.patientName = name;
        this.age = age;
        this.weight = weight;
        this.height = height;
        this.gender = gender;
        this.occupation = occupation;
        this.preference = pref;
        this.allergies = allergies;
        this.meals = meals;
        this.activityLevel = ActivityLevel.NIL;
        this.calorie = (gender == Gender.Male) ? (int)((10 * weight) + (6.25 * height) - (5 * age) + 5) : (int)((10 * weight) + (6.25 * height) - (5 * age) - 161);
    }

    public int GetCurrentCalories()
    {
        double currentCalories = 0;
        foreach(Dish meal in meals)
        {
            currentCalories += meal.calories;
        }
        return (int)currentCalories;
    }
}
