using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [Header("Header")]
    [SerializeField]
    private Transform _headerArea;
    [SerializeField]
    private TextMeshProUGUI _titleField;

    [Header("Content")]
    [SerializeField]
    private Transform _contentArea;
    [SerializeField]
    private Transform _verticalLayoutArea;
    [SerializeField]
    private Transform _heroImageContainer;
    [SerializeField]
    private Image _heroImage;
    [SerializeField]
    private TextMeshProUGUI _heroText; 
    [Space()]
    [SerializeField]
    private Transform _horizontalLayoutArea;
    [SerializeField]
    private Transform _iconContainer;
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private TextMeshProUGUI _iconText;

    [Header("Footer")]
    [SerializeField]
    private Transform _footerArea;
    [SerializeField]
    private Button _confirmButton;
    [SerializeField]
    private Button _declineButton;
    [SerializeField]
    private Button _alternateButton;

    private Action onConfirmCallback;
    private Action onDeclineCallback;
    private Action onAlternateCallback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Confirm()
    {
        onConfirmCallback?.Invoke();
        Close();
    }
    public void Decline()
    {
        onDeclineCallback?.Invoke();
        Close();
    }
    public void Alternate()
    {
        onAlternateCallback?.Invoke();
        Close();
    }

    public void Close()
    {

    }

    public void ShowAsVertical(string title, Sprite imageToShow, string body, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        _horizontalLayoutArea.gameObject.SetActive(false);

        _headerArea.gameObject.SetActive(string.IsNullOrEmpty(title));
        _titleField.text = title;

        _heroImage.sprite = imageToShow;
        _heroText.text = body;

        onConfirmCallback = confirmAction;
        _declineButton.gameObject.SetActive(declineAction == null);
        _alternateButton.gameObject.SetActive(alternateAction == null);
        onDeclineCallback = declineAction;
        onAlternateCallback = alternateAction;
    }
}
