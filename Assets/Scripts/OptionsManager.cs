using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public GameObject popUpWindow;
    public Sprite toggleOn;
    public Sprite toggleOff;
    public GameObject musicOnUI;
    public GameObject musicOffUI;
    public GameObject sfxOnUI;
    public GameObject sfxOffUI; 


    private void Start()
    {
        musicOnUI.SetActive(GameManager.musicEnabled);
        musicOffUI.SetActive(!GameManager.musicEnabled); 
        sfxOnUI.SetActive(GameManager.sfxEnabled);
        sfxOffUI.SetActive(!GameManager.sfxEnabled);
    }

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
        GameManager.musicEnabled = !GameManager.musicEnabled;
        musicOnUI.SetActive(GameManager.musicEnabled);
        musicOffUI.SetActive(!GameManager.musicEnabled);
    }

    public void OnToggleSfx()
    {
        GameManager.sfxEnabled = !GameManager.sfxEnabled;
        sfxOnUI.SetActive(GameManager.sfxEnabled);
        sfxOffUI.SetActive(!GameManager.sfxEnabled);
    }

    public void OnExitToMainMenu()
    {
        // call save
        GameManager.SaveGame();
        SceneManager.LoadScene(0);
    }
}
