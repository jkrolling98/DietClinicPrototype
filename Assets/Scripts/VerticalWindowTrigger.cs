using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWindowTrigger : MonoBehaviour
{
    public string title;
    public Sprite sprite;
    public string body;
    public bool triggerOnEnable;

    public void OnEnable()
    {
        if(!triggerOnEnable) { return; }

        UIController.Instance.ModalWindow.ShowAsVertical(title,sprite,body,null);
    }
}
