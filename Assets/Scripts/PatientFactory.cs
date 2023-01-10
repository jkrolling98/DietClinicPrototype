using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientFactory : MonoBehaviour
{

    public static PatientFactory instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
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

    public Patient CreateNewStudent()
    {
        if(Random.Range(0, 2) == 0)
        {
            string name = RandomMaleName();
            int age = Random.Range(11, 20);
            int weight = Random.Range(45, 100);
            int height = Random.Range(140, 190);
            Patient.FoodPref pref = Patient.FoodPref.NIL; //to be changed
            Patient.Allergies allergies = Patient.Allergies.NIL; //to be changed
            return new Patient(name, age, weight, height, Patient.Gender.Male, Patient.Occupation.Student, pref, allergies);
        }
        else
        {
            string name = RandomFemaleName();
            int age = Random.Range(11, 20);
            int weight = Random.Range(40, 80);
            int height = Random.Range(130, 180);
            Patient.FoodPref pref = Patient.FoodPref.NIL; //to be changed
            Patient.Allergies allergies = Patient.Allergies.NIL; //to be changed
            return new Patient(name, age, weight, height, Patient.Gender.Female, Patient.Occupation.Student, pref, allergies);
        }
    }
}
