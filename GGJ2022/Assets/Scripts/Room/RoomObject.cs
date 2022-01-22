using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public int sanity, choiceOneSanity, choiceTwoSanity, unlocksRoom;


    private GameController gameController;

    void Start()
    {
        // AYTO THA TREXEI OTAN TO AKOUMPAEI O XRISTIS MIA FORA
        // gameManager.GetComponent<GameController>().changeSanityLevel(roomSanity);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
