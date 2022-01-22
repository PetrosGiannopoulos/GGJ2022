using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNumber, roomSanity;
    public Transform spawnPoint, object1Point, object2Point, objectKeyPoint, objectDestroyerPoint;
    public GameObject object1, object2, objectKey, objectDestroyer;


    private GameController gameController;

    void Start()
    {
        // AYTO THA PAEI STO GAME MANAGER, MOLIS METAFEROUME TON PLAYER STO SPAWNPOINT
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
