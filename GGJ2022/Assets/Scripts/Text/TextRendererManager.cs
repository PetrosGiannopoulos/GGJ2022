using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GGJ.CK;
using UnityEngine.SceneManagement;

public class TextRendererManager : MonoBehaviour
{

    public static TextRendererManager instance;
    public Canvas textCanvas;
    public TextMeshProUGUI textForm;
    public TextMeshProUGUI storyForm;
    public TextMeshProUGUI endingForm;
    public Image blackScreen;

    public Image continueImage;
    public TextMeshProUGUI continueText;

    public List<TextAsset> herculesGoodText = new List<TextAsset>();
    public List<TextAsset> herculesBadText = new List<TextAsset>();

    public List<TextAsset> pickupText = new List<TextAsset>();
    public List<TextAsset> endingText = new List<TextAsset>();
    

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
        GameController.instance.playerHud.SetActive(false);
        textCanvas.enabled = true;
        ShowContinue();
    }

    public void EndDialogScene()
    {
        //textCanvas.enabled = false;
        HideContinue();
        GameController.instance.playerHud.SetActive(true);
    }

    public void SetPickupText(string name)
    {
        
        foreach (TextAsset text in pickupText)
        {
            if (text.name.Equals(name))
            {
                StopAllCoroutines();
                RenderText(text.text, 6, 1);
                
                return;
            }
        }

    }

    public void ResetPickupText()
    {
        StopAllCoroutines();
        storyForm.text = "";
        Canvas.ForceUpdateCanvases();
        storyForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        storyForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

    }

    public void SetEndingText(string name, float duration)
    {
        foreach (TextAsset text in endingText)
        {
            if (text.name.Contains(name))
            {
                StopAllCoroutines();
                RenderText(text.text, duration, 2);

                return;
            }
        }
    }

    public void ResetEndingText()
    {
        StopAllCoroutines();
        endingForm.text = "";
        Canvas.ForceUpdateCanvases();
        endingForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        endingForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

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

        //RenderText(lorem, 3,1);
        //RenderTypedText(lorem, 3);
        //RenderMultipleTypedText(phrases, 3,1);
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
        GameController.instance.player.GetComponent<PlayerMovement>().enabled = false;
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

    public void RenderText(string text, float duration,int type=0)
    {
        StartCoroutine(DelayText(text,duration,type));
    }

    IEnumerator DelayText(string text, float duration,int type=0)
    {

        TextMeshProUGUI dynamicTextForm = null;

        if (type == 0) dynamicTextForm = textForm;
        else if (type == 1) dynamicTextForm = storyForm;
        else dynamicTextForm = endingForm;

        dynamicTextForm.horizontalAlignment = HorizontalAlignmentOptions.Center;
        dynamicTextForm.text = text;

        Canvas.ForceUpdateCanvases();
        dynamicTextForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        dynamicTextForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;


        yield return new WaitForSeconds(duration);
        dynamicTextForm.text = "";
        dynamicTextForm.horizontalAlignment = HorizontalAlignmentOptions.Left;

        Canvas.ForceUpdateCanvases();
        dynamicTextForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        dynamicTextForm.gameObject.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;


        if (type == 2)
        {
            SceneManager.LoadScene("CreditsScene");
        }

    }

    
    public void RenderTypedText(string text, float duration, int type=0)
    {

        StartCoroutine(DelayTyping(text,renderDelayTime, duration,type));
        
    }

    IEnumerator DelayTyping(string text, float delayTime, float duration,int type=0)
    {

        TextMeshProUGUI dynamicTextForm = null;
        if (type == 0) dynamicTextForm = textForm;
        else if (type == 1) dynamicTextForm = storyForm;
        else dynamicTextForm = endingForm;

        dynamicTextForm.text = "";
        Debug.Log("Typing");
        foreach (char c in text)
        {
            yield return new WaitForSeconds(delayTime);
            dynamicTextForm.text += c;
        }

        yield return new WaitForSeconds(duration);

        isTextDisplayed = true;
        
    }

    IEnumerator DelayMultipleTyping(List<string> multipleText, float delayTime, float duration,int type)
    {
        InitDialogScene();
        TextMeshProUGUI dynamicTextForm = null; ;
        if (type == 0) dynamicTextForm = textForm;
        else if (type == 1) dynamicTextForm = storyForm;
        else dynamicTextForm = endingForm;

        for (int i = 0; i < multipleText.Count; i++)
        {
            string s = multipleText[i];

            dynamicTextForm.text = "";
            foreach (char c in s)
            {
                yield return new WaitForSeconds(delayTime);
                dynamicTextForm.text += c;
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

        blackScreen.color = new Color(0,0,0,0);
        dynamicTextForm.text = "";
        GameController.instance.player.GetComponent<PlayerMovement>().enabled = true;
        AudioManager.instance.StopCurrent();
        AudioManager.instance.PlayFadeIn("MainTheme");

        EndDialogScene();
    }

    public void RenderMultipleTypedText(List<string> multipleText, float duration, int type=0)
    {
        StartCoroutine(DelayMultipleTyping(multipleText, 0.08f,duration,type));
    }
}
