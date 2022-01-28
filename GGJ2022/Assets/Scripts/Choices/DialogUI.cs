using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private GameController gameController;
    public GameObject dialogTextPrefab;
    private string currentGameObjectName;
    List<GameObject> dialogTextObjs = new List<GameObject>();
    private int selectionIndex = 0;
    bool isKeyReset = false;
    // Start is called before the first frame update
    void Start()
    {
        //AddDialogChoice("Whatever Dialog");

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            Debug.Log("Found");
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

    }

    public void AddDialogChoice(string dialogText,string itemName, int maxDialogs)
    {
        currentGameObjectName = itemName;
        //if (dialogTextObjs.Count >= maxDialogs) ClearDialogs();
        //Debug.Log($"ItemName: {itemName}, DialogCount: {maxDialogs}");

        var dialog = Instantiate(dialogTextPrefab, transform);

        GameObject dialogTextObj = dialog.transform.GetChild(0).gameObject;
        dialogTextObj.GetComponent<TextMeshProUGUI>().text = dialogText;

        if (dialogTextObjs.Count == 0) dialogTextObj.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;

        dialogTextObjs.Add(dialog);
    }

    public void ResetKeyState()
    {
        StartCoroutine(delayResetKey());
    }

    IEnumerator delayResetKey()
    {
        yield return new WaitForEndOfFrame();

        isKeyReset = true;
    }

    public void SetSelection(int index)
    {
        if (dialogTextObjs.Count == 0) return;
        selectionIndex = index;
        DeselectDialogs();
        dialogTextObjs[selectionIndex].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
    }

    public void DeselectDialogs()
    {
        foreach (GameObject go in dialogTextObjs)
        {
            go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }
    }

    public void ClearDialogs()
    {
        foreach (GameObject go in dialogTextObjs) Destroy(go);
        dialogTextObjs.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSelection(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (dialogTextObjs.Count >2)
            {
                SetSelection(2);
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (dialogTextObjs.Count > 3) SetSelection(3);
        }


        if (Input.GetKeyDown(KeyCode.Return) && dialogTextObjs.Count>0 && isKeyReset)
        {
            //Close Dialog and Implement Choice Result
            //Debug.Log(selectionIndex);
            int dialogCount = dialogTextObjs.Count;
            ClearDialogs();
            isKeyReset = false;

            
            ImplementChoice(dialogCount);

        }
    }

    private void ImplementChoice(int dialogCount)
    {
        if (currentGameObjectName == "Belt" && selectionIndex == 0)
        {

            //Destroy Belt
            gameController.secondRoomIsGood = true;
            StorySanity.instance.AddSanityPoints(+5);
            GameObject belt = GameObject.Find(currentGameObjectName);
            GameObject throwAwayPoint = null;
            
            for (int i = 0; i < belt.transform.childCount; i++)
            {
                GameObject childGO = belt.transform.GetChild(i).gameObject;
                if (childGO.name.Equals("ThrowAwayPoint")) throwAwayPoint = childGO;
                
            }

            belt.transform.position = throwAwayPoint.transform.position;
            LocationManager.instance.defaultRoom1 = false;
            DestroyInteraction();

        }
        else if (currentGameObjectName == "Belt" && selectionIndex == 1)
        {
            //Don't destroy it
            gameController.secondRoomIsGood = false;
            StorySanity.instance.AddSanityPoints(-10);
            DestroyInteraction();
           
        }
        
        if (currentGameObjectName == "PhoneClean" && selectionIndex==0)
        {
            StorySanity.instance.AddSanityPoints(-10);
            DestroyInteraction();
            
        }
        else if(currentGameObjectName == "PhoneClean" && selectionIndex == 1)
        {
           StorySanity.instance.AddSanityPoints(5);
            DestroyInteraction();
        }

        if(currentGameObjectName == "DrugsMessy" && selectionIndex == 0)
        {
            StorySanity.instance.AddSanityPoints(15);
            DestroyInteraction();
        }
        else if (currentGameObjectName == "DrugsMessy" && selectionIndex == 1)
        {
            StorySanity.instance.AddSanityPoints(-15);
            DestroyInteraction();
        }

        if(currentGameObjectName == "DeadBodyCovered")
        {
            
            if (dialogCount == 3)
            {
                
                if (selectionIndex == 0)
                {
                   
                    //Do nothing
                    StorySanity.instance.AddSanityPoints(-10);
                    gameController.TeleportPlayer("Theater");
                    DestroyInteraction();
                    

                }
                else if(selectionIndex == 1)
                {
                    //Pickpocket and Hide
                    StorySanity.instance.AddSanityPoints(-15);
                    gameController.TeleportPlayer("Theater");
                    DestroyInteraction();
                    
                }
                
            }
            else if(dialogCount == 4)
            {
               
                if (selectionIndex == 0)
                {
                    
                    //Report to police
                    StorySanity.instance.AddSanityPoints(10);
                    gameController.TeleportPlayer("Theater");
                    DestroyInteraction();
                }
                else if (selectionIndex == 1)
                {
                    //Do nothing
                    gameController.TeleportPlayer("Theater");
                    DestroyInteraction();
                }
                else if(selectionIndex == 2)
                {
                    //Pickpocket
                    StorySanity.instance.AddSanityPoints(-10);
                    gameController.TeleportPlayer("Theater");
                    DestroyInteraction();
                }
            }
        
        }

        currentGameObjectName = "";
    }

    public void DestroyInteraction()
    {
        GameObject pickupObject = null;
        GameObject interactionObj = GameObject.Find(currentGameObjectName);
        for (int i = 0; i < interactionObj.transform.childCount; i++)
        {
            GameObject childGO = interactionObj.transform.GetChild(i).gameObject;
            if (childGO.name.Equals("PickupTrigger")) pickupObject = childGO;

        }
        Destroy(pickupObject, 0.1f);
    }
}
