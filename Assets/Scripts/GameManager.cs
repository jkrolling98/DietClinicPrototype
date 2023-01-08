using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject patient;
    public Text patientInfo;
    public GameObject IngredientA;
    public GameObject IngredientB;
    public GameObject IngredientC;
    public GameObject MealWindow;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate new patient and update patient info tab
    }

    public void Serve()
    {
        //int valueA = int.Parse(IngredientA.GetComponent<Text>().text);
        //int valueB = int.Parse(IngredientB.GetComponent<Text>().text);
        //int valueC = int.Parse(IngredientC.GetComponent<Text>().text);
        //MealWindow.ge

        //if (valueA + valueB + valueC < 1)
        //{
        //    Debug.Log("insufficient Ingredients!");
        //}
        //else
        //{
        //    Debug.Log("Dish served!");
        //}
        PlusMinusButton[] pickers;
        pickers = MealWindow.GetComponentsInChildren<PlusMinusButton>();
        for (int i = 0; i < pickers.Length; i++)
            {
                Debug.Log($"Ingredient {i+1} value: {pickers[i].currentValue}");
            }
    }
}
