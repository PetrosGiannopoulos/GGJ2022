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

    public void AddDialogChoice(string dialogText,string itemName)
    {
        currentGameObjectName = itemName;
        if (dialogTextObjs.Count >= 2) ClearDialogs();

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


        if (Input.GetKeyDown(KeyCode.E) && isKeyReset)
        {
            //Close Dialog and Implement Choice Result
            Debug.Log(selectionIndex);
            ClearDialogs();
            isKeyReset = false;

            ImplementChoice();

        }
    }

    private void ImplementChoice()
    {
        if (currentGameObjectName == "Belt" && selectionIndex == 0) gameController.secondRoomIsGood = true;
        else if (currentGameObjectName == "Belt" && selectionIndex == 1) gameController.secondRoomIsGood = false;
    }
}
