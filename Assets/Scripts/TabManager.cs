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
    public GameObject daySummaryTab;

    public static TabManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
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

    public void ViewDaySummary()
    {
        EnableTab(daySummaryTab);
    }

    void EnableTab(GameObject tab)
    {
        helpTab.SetActive(false);
        infoTab.SetActive(false);
        pastMealsTab.SetActive(false);
        deviseMealsTab.SetActive(false);
        summaryTab.SetActive(false);
        daySummaryTab.SetActive(false);
        tab.SetActive(true);
    }
}
