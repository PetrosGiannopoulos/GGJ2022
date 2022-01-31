using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GGJ.CK;

public class UIInteraction : MonoBehaviour
{

    public Image fadeInImage;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.StopCurrent();
        AudioManager.instance.PlayFadeIn("Lucifer");
    }

    public void StartGame()
    {
        StartCoroutine(FadeInTransition());
        
    }

    IEnumerator FadeInTransition()
    {

        float aStep = 0.01f;
        float endValue = 1;
        int nIter = (int)(((float)endValue) / ((float)aStep));
        for(int i = 0; i < nIter; i++)
        {
            fadeInImage.color += new Color(0,0,0,aStep);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        fadeInImage.color = new Color(0,0,0,1);
        
        SceneManager.LoadScene("DisclaimerScene");
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
        
    }
}
