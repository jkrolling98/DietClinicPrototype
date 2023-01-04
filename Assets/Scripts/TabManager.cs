using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject pastMealsTab;
    public GameObject deviseMealsTab;
    public void ViewPastMeals()
    {
        EnableTab(pastMealsTab);
    }

    public void ViewInfo()
    {
        EnableTab(infoTab);
    }

    public void ViewDeviseMeal()
    {
        EnableTab(deviseMealsTab);
    }

    void EnableTab(GameObject tab)
    {
        infoTab.SetActive(false);
        pastMealsTab.SetActive(false);
        deviseMealsTab.SetActive(false);
        tab.SetActive(true);
    }
}
