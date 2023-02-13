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
        Very
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
        if(gender == Gender.Male)
        {
            this.calorie = (int)(66.5 + (13.75 * weight) + (5.003 * height) - (6.75 * age));
        }
        else
        {
            this.calorie = (int)(655.1 + (9.563 * weight) + (1.850 * height) - (4.676 * age));
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
    }
}
