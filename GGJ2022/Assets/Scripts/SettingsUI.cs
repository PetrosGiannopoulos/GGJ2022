using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GGJ.CK;

public class SettingsUI : MonoBehaviour
{

    public Canvas canvas;
    public Button mainMenuButton;
    public Button quitButton;
    public CodeBoard codeBoard;

    int selection = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        

        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && codeBoard.interacting==false)
        {
            ToggleCanvas();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && canvas.enabled)
        {
            selection = 0;
            mainMenuButton.GetComponent<Image>().color = new Color(0, 0, 0);
            quitButton.GetComponent<Image>().color = new Color(1, 1, 1);

        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && canvas.enabled)
        {
            selection = 1;
            mainMenuButton.GetComponent<Image>().color = new Color(1, 1, 1);
            quitButton.GetComponent<Image>().color = new Color(0, 0, 0);
        }

        if(canvas.enabled && Input.GetKeyDown(KeyCode.Return))
        {
            if (selection == 0) GoToMainMenu();
            else QuitGame();
        }
    }

    private void ToggleCanvas()
    {
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled)
        {
            Time.timeScale = 0f;
            mainMenuButton.GetComponent<Image>().color = new Color(0, 0, 0);
            quitButton.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else Time.timeScale = 1f;
    }
}
