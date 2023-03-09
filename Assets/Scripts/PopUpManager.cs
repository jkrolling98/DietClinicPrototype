using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpWindow;
    public GameObject helpWindow = null;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;

    // for round summary
    public GameObject star1 = null;
    public GameObject star2 = null;
    public GameObject star3 = null;

    public IEnumerator WaitForClose()
    {
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public IEnumerator WaitForClose(int count)
    {
        if (star1 != null && star2 != null && star3 != null)
        {
            if (count >= 1) LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
            if (count >= 2) LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
            if (count >= 3) LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.3f).setEase(LeanTweenType.easeOutElastic);
        }
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public void OnClose()
    {
        popUpWindow.SetActive(false);
    }


    public void toggleHelp()
    {
        helpWindow.SetActive(!helpWindow.activeSelf);
    }
}
