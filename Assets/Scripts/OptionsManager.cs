using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    public GameObject popUpWindow;

    public IEnumerator WaitForClose()
    {
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public void OnClose()
    {
        popUpWindow.SetActive(false);
    }

    public void OnToggleMusic()
    {
        
    }

    public void OnToggleSfx()
    {

    }

    public void OnExitToMainMenu()
    {


    }
}
