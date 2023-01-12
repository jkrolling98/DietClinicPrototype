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
    public Meal[] meals;

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
        Crustacean,
        Peanuts
    }

    public Patient(string name, int age, int weight, int height, Gender gender, Occupation occupation, FoodPref pref, Allergies allergies, Meal[] meals)
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
    }
}
