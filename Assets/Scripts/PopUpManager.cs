using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpWindow;
    public GameObject helpWindow;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;

    public void OnClose()
    {
        Destroy(popUpWindow);
    }

    public void toggleHelp()
    {
        helpWindow.SetActive(!helpWindow.activeSelf);
    }
}
