using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeBoard : MonoBehaviour
{

    public GameObject codeBoardTextObj;

    private int passCode = 516794;

    public bool interacting = false;
    List<KeyCode> numberKeyCodes = new List<KeyCode>();
    public GameObject playerObj;
    private float safeDist = 3f;
    public GameObject portalVFXPrefab;
    GameObject portalVFX;
    

    // Start is called before the first frame update
    void Start()
    {
        numberKeyCodes.Add(KeyCode.Alpha0);
        numberKeyCodes.Add(KeyCode.Alpha1);
        numberKeyCodes.Add(KeyCode.Alpha2);
        numberKeyCodes.Add(KeyCode.Alpha3);
        numberKeyCodes.Add(KeyCode.Alpha4);
        numberKeyCodes.Add(KeyCode.Alpha5);
        numberKeyCodes.Add(KeyCode.Alpha6);
        numberKeyCodes.Add(KeyCode.Alpha7);
        numberKeyCodes.Add(KeyCode.Alpha8);
        numberKeyCodes.Add(KeyCode.Alpha9);
    }

    // Update is called once per frame
    void Update()
    {
        if (interacting)
        {
            for(int i=0;i<numberKeyCodes.Count;i++)
            {
                KeyCode key = numberKeyCodes[i];
                if (Input.GetKeyDown(key))
                {
                    AddNumber(i);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) ResetButton();
            if (Input.GetKeyDown(KeyCode.Return)) EnterButton();
        }



        if (Vector3.Distance(playerObj.transform.position, transform.position) < safeDist)
        {
            interacting = true;
            transform.parent.gameObject.GetComponent<Outline>().enabled = true;
            TextRendererManager.instance.SetPickupText("CodeHint");
        }
        else
        {
           
            transform.parent.gameObject.GetComponent<Outline>().enabled = false;
            if(interacting)TextRendererManager.instance.ResetPickupText();

            interacting = false;

        }
    }

    public void AddNumber(int n)
    {
        
        string codeText = codeBoardTextObj.GetComponentInChildren<TextMeshProUGUI>().text;
        
        if (codeText.Length < 6)
        {
            codeBoardTextObj.GetComponentInChildren<TextMeshProUGUI>().text += "" + n;
        }
    }

    public void ResetButton()
    {
        codeBoardTextObj.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public void EnterButton()
    {
        string codeText = codeBoardTextObj.GetComponentInChildren<TextMeshProUGUI>().text;
        int codeResult = int.Parse(codeText);

        Color numberColor = Color.black;

        if (codeResult == passCode)
        {
            Debug.Log("Code Correct");

            numberColor = new Color(0, 0.435f, 0.227f);
            if (portalVFX == null)
            {
                portalVFX = Instantiate(portalVFXPrefab);
            }

            

        }
        else
        {
            Debug.Log("Code Wrong");
            numberColor = new Color(1, 0.012f, 0);
        }

        StartCoroutine(FlashColors(numberColor));
    }

    IEnumerator FlashColors(Color color)
    {
        var codeParent = codeBoardTextObj.transform.parent.gameObject;
        GameObject[] numberObjs = GameObject.FindGameObjectsWithTag("NumberButton");

        float flashTime = 2.0f;
        List<Color> buColors = new List<Color>();
        foreach (GameObject go in numberObjs)
        {
            buColors.Add(go.GetComponent<Image>().color);
            go.GetComponent<Image>().color = color;
        }


        yield return new WaitForSeconds(flashTime);
        

        for(int i=0;i<numberObjs.Length;i++)
        {
            GameObject go = numberObjs[i];
            go.GetComponent<Image>().color = buColors[i];
        }
        
    }
}
