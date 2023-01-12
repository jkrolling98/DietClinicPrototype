using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int minimum;
    public int maximum;
    public int current;
    public Image mask;
    public TextMeshProUGUI statusText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getCurrentFill();
    }

    void getCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
        statusText.text = current + "/" + maximum;
    }
}
