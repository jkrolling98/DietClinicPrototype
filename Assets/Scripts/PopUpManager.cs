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
    public GameObject patientUI = null;
    public GameObject speechBubble = null;
    public GameObject emotes = null;

    // for round summary
    public GameObject star1 = null;
    public GameObject star2 = null;
    public GameObject star3 = null;

    public IEnumerator WaitForClose()
    {
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public IEnumerator WaitForClose(int count, string emotion)
    {
        if(patientUI != null)
        {
            // set patient sprite
            if (GameManager.currentPatient.gender == Patient.Gender.Male)
            {
                patientUI.GetComponent<Image>().sprite = GameManager.currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Male_01/" + emotion) : Resources.Load<Sprite>($"Patients/Male_02/" + emotion);
            }
            else
            {
                patientUI.GetComponent<Image>().sprite = GameManager.currentPatient.height % 2 == 0 ? Resources.Load<Sprite>($"Patients/Female_01/" + emotion) : Resources.Load<Sprite>($"Patients/Female_02/" + emotion);
            }
            // set emote
            string emoteName = "";
            switch (emotion)
            {
                case ("upset"):
                    emoteName = "E34";
                    break;
                case ("neutral"):
                    emoteName = "E25";
                    break;
                case ("happy"):
                    emoteName = "E1";
                    break;
                case ("elated"):
                    emoteName = "E11";
                    break;
            }
            emotes.GetComponent<Image>().sprite = Resources.Load<Sprite>("Emotes/" + emoteName);
            emotes.SetActive(true);
            speechBubble.SetActive(true);
        }
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
