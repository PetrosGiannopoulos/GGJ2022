using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{

    public GameObject dialogTextPrefab;
    List<GameObject> dialogTextObjs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //AddDialogChoice("Whatever Dialog");
    }

    public void AddDialogChoice(string dialogText)
    {
        var dialog = Instantiate(dialogTextPrefab,transform);

        dialog.GetComponent<TextMeshProUGUI>().text = dialogText;

        dialogTextObjs.Add(dialog);
    }

    public void ClearDialogs()
    {
        dialogTextObjs.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
