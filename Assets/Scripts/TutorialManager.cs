using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject popUpWindow;
    private int count;
    public int current = 1;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI footerText;
    public Image bodyImage;

    [TextArea]
    public List<string> bodyTexts;
    public List<Sprite> tutorialImages;

    public GameObject prevBtn;
    public GameObject nextBtn;
    public GameObject doneBtn;
    public GameObject skipBtn;

    public void Start()
    {
        count = bodyTexts.Count;
    }
    public void Update()
    {
        bodyImage.gameObject.SetActive(tutorialImages[current - 1]!=null);
        bodyImage.sprite = tutorialImages[current-1];
        bodyText.text = bodyTexts[current-1];
        footerText.text = $"{current}/{count}";
        prevBtn.SetActive(current>1);
        nextBtn.SetActive(current<count);
        doneBtn.SetActive(current == count);
        skipBtn.SetActive(current < count);
    }

    public IEnumerator WaitForClose()
    {
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public IEnumerator WaitForClose(int count)
    {
        yield return new WaitUntil(() => !popUpWindow.activeSelf);

        Destroy(popUpWindow);
    }

    public void OnDone()
    {
        popUpWindow.SetActive(false);
    }
    public void OnPrev()
    {
        current--;
    }

    public void OnNext()
    {
        current++;
    }

}
