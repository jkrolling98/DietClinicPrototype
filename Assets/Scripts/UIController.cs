using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField]
    private ModalWindow _modalWindow;
    public ModalWindow ModalWindow => _modalWindow;

    private void Awake()
    {
        Instance = this;
    }
}
