using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextRendererManager : MonoBehaviour
{

    public static TextRendererManager instance;
    public Canvas textCanvas;
    public TextMeshProUGUI textForm;
    public Image blackScreen;

    public Image continueImage;
    public TextMeshProUGUI continueText;

    public List<TextAsset> herculesGoodText = new List<TextAsset>();
    public List<TextAsset> herculesBadText = new List<TextAsset>();

    bool isTextDisplayed = false;
    bool isSkipped = false;

    public float renderDelayTime=0.05f;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void InitDialogScene()
    {
        textCanvas.enabled = true;
        ShowContinue();
    }

    public void EndDialogScene()
    {
        textCanvas.enabled = false;
        HideContinue();
    }

    public void ShowContinue()
    {
        continueImage.enabled = true;
        continueText.enabled = true;
    }

    public void HideContinue()
    {
        continueImage.enabled = false;
        continueText.enabled = false;
    }

    public void InitHerculesStatueGoodText()
    {
        //Debug.Log(hercules123.text);
        //BlackenScreenAndRenderTypedText(hercules123.text, 3, 1);
        List<string> multipleText = new List<string>();
        foreach (TextAsset textAsset in herculesGoodText) multipleText.Add(textAsset.text);
        BlackenScreenAndRenderMultipleTypedText(multipleText, 3, 1);
    }

    public void InitHerculesStatueBadText()
    {
        //Debug.Log(hercules123.text);
        //BlackenScreenAndRenderTypedText(hercules123.text, 3, 1);
        List<string> multipleText = new List<string>();
        foreach (TextAsset textAsset in herculesBadText) multipleText.Add(textAsset.text);
        BlackenScreenAndRenderMultipleTypedText(multipleText, 3, 1);
    }

    string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam sit amet gravida enim. Ut pulvinar dui aliquam erat consectetur pharetra a mattis lectus. Donec finibus, nibh in pellentesque iaculis, lacus elit auctor eros, ac fringilla lacus turpis at turpis. Aliquam erat volutpat. In pharetra lobortis sapien, a hendrerit nunc maximus vitae";

    List<string> phrases = new List<string>();
    
    private void Start()
    {

        phrases.Add("Lorem ipsum dolor sit amet");
        phrases.Add(", consectetur adipiscing elit.");
        phrases.Add("Etiam sit amet gravida enim. Ut pulvina");
        phrases.Add("r dui aliquam erat consectetur pharetra a m");
        phrases.Add(", nibh in pellentesque iaculis, lacus eli");

        //RenderText("Hello World", 3);
        //RenderTypedText(lorem, 3);
        //RenderMultipleTypedText(phrases, 3);
        //BlackenScreenAndRenderTypedText(lorem, 3, 1);
        //BlackenScreenAndRenderMultipleTypedText(phrases, 3, 1);
        //InitHerculesStatueGoodText();
    }
    // Update is called once per frame
    void Update()
    {
        if(textCanvas.enabled && Input.GetKeyDown(KeyCode.F))if (continueImage.enabled)isSkipped = true;
    }

    public void BlackenScreenAndRenderTypedText(string text, float textDuration, float blackScreenDuration)
    {
        StartCoroutine(DelayBlackeningAndRenderTypedText(text,0.05f,textDuration,blackScreenDuration));
    }

    public void BlackenScreenAndRenderMultipleTypedText(List<string> multipleText, float textDuration, float blackScreenDuration)
    {
        StartCoroutine(DelayBlackeningAndRenderMultipleTypedText(multipleText, renderDelayTime, textDuration, blackScreenDuration));
    }

    public void BlackenScreen(float duration)
    {
        StartCoroutine(DelayBlackening(duration, renderDelayTime));
    }

    IEnumerator DelayBlackening(float duration, float delayTime)
    {

        int timeSteps = (int)(duration / delayTime);
        //time goes from 0->timesteps
        //a goes from 0-1
        for(int i = 0; i < timeSteps; i++)
        {
            float aValue = ((float)i) / ((float)timeSteps);
            blackScreen.GetComponent<Image>().color = new Color(0,0,0,aValue);
            yield return new WaitForSeconds(delayTime);
        }


        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);

    }

    IEnumerator DelayBlackeningAndRenderTypedText(string text, float delayTime, float textDuration, float blackScreenDuration)
    {

        int timeSteps = (int)(blackScreenDuration / delayTime);
        //time goes from 0->timesteps
        //a goes from 0-1
        for (int i = 0; i < timeSteps; i++)
        {
            float aValue = ((float)i) / ((float)timeSteps);
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, aValue);
            yield return new WaitForSeconds(delayTime);
        }

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        RenderTypedText(text, textDuration);

    }

    IEnumerator DelayBlackeningAndRenderMultipleTypedText(List<string> multipleText, float delayTime, float textDuration, float blackScreenDuration)
    {

        int timeSteps = (int)(blackScreenDuration / delayTime);
        //time goes from 0->timesteps
        //a goes from 0-1
        for (int i = 0; i < timeSteps; i++)
        {
            float aValue = ((float)i) / ((float)timeSteps);
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, aValue);
            yield return new WaitForSeconds(delayTime);
        }

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        RenderMultipleTypedText(multipleText, textDuration);

    }

    public void RenderText(string text, float duration)
    {
        StartCoroutine(DelayText(text,duration));
    }

    IEnumerator DelayText(string text, float duration)
    {
        textForm.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textForm.text = text;
        yield return new WaitForSeconds(duration);
        textForm.text = "";
        textForm.horizontalAlignment = HorizontalAlignmentOptions.Left;
    }

    
    public void RenderTypedText(string text, float duration)
    {

        StartCoroutine(DelayTyping(text,renderDelayTime, duration));
        
    }

    IEnumerator DelayTyping(string text, float delayTime, float duration)
    {
        textForm.text = "";
        Debug.Log("Typing");
        foreach (char c in text)
        {
            yield return new WaitForSeconds(delayTime);
            textForm.text += c;
        }

        yield return new WaitForSeconds(duration);

        isTextDisplayed = true;
        
    }

    IEnumerator DelayMultipleTyping(List<string> multipleText, float delayTime, float duration)
    {
        InitDialogScene();


        for (int i = 0; i < multipleText.Count; i++)
        {
            string s = multipleText[i];

            textForm.text = "";
            foreach (char c in s)
            {
                yield return new WaitForSeconds(delayTime);
                textForm.text += c;
                if (isSkipped)break;
            }

            isTextDisplayed = true;

            if (isSkipped == false)
            {
                HideContinue();
                yield return new WaitForSeconds(duration);
                ShowContinue();
            }
            else
            {
                isSkipped = false;

                if (i == multipleText.Count - 1) EndDialogScene();
            }
            
        }

        EndDialogScene();
    }

    public void RenderMultipleTypedText(List<string> multipleText, float duration)
    {
        StartCoroutine(DelayMultipleTyping(multipleText, renderDelayTime,duration));
    }
}
