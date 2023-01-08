using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject helpTab;
    public GameObject infoTab;
    public GameObject pastMealsTab;
    public GameObject deviseMealsTab;
    public GameObject summaryTab;

    public static TabManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ViewHelp()
    {
        EnableTab(helpTab);
    }

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

    public void ViewSummary()
    {
        EnableTab(summaryTab);
    }

    void EnableTab(GameObject tab)
    {
        helpTab.SetActive(false);
        infoTab.SetActive(false);
        pastMealsTab.SetActive(false);
        deviseMealsTab.SetActive(false);
        summaryTab.SetActive(false);
        tab.SetActive(true);
    }
}
