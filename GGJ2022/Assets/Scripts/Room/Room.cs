using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject gameManager;
    public int roomNumber, roomSanity;
    public Transform spawnPoint, object1Point, object2Point, objectKeyPoint;
    public GameObject object1, object2, objectKey;

    // Start is called before the first frame update
    void Start()
    {
        // AYTO THA PAEI STO GAME MANAGER, MOLIS METAFEROUME TON PLAYER STO SPAWNPOINT
        // gameManager.GetComponent<GameController>().changeSanityLevel(roomSanity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
