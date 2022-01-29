using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisclaimerManager : MonoBehaviour
{

    public Image fadeOutImage;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(FadeOutTransition(5f));

    }

    IEnumerator FadeOutTransition(float delayTime)
    {

        float aStep = 0.01f;
        float startValue = 1;
        int nIter = (int)(((float)startValue) / ((float)aStep));
        for (int i = 0; i < nIter; i++)
        {
            fadeOutImage.color -= new Color(0, 0, 0, aStep);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        fadeOutImage.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(delayTime);
        fadeOutImage.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
