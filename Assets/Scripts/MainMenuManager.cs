using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private RawImage background;
    [SerializeField]
    private float _x, _y;
    public Canvas canvas;
    public GameObject optionsWindow;

    private void Update()
    {
        background.uvRect = new Rect( background.uvRect.position + new Vector2( _x, _y ) * Time.deltaTime,background.uvRect.size );
        if(background.uvRect.x >= 0.5) { _x = -_x; }
        if(background.uvRect.x <= -0.1) { _x = -_x; }
        if (background.uvRect.y >= 0.1) { _y = -_y; }
        if (background.uvRect.y <= -0.1) { _y = -_y; }
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnContinue()
    {

    }

    public void OnSettings()
    {
        StartCoroutine(OpenOptionsWindow());
    }

    public IEnumerator OpenOptionsWindow()
    {
        GameObject popUp = Instantiate(optionsWindow, canvas.transform.position, Quaternion.identity, canvas.transform);
        yield return StartCoroutine(popUp.GetComponent<OptionsManager>().WaitForClose());
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
