using GGJ.CK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int sanityMeter;
    public List<Transform> teleportLocations = new List<Transform>();
    public Transform museumPlayerTransform;
    private int nextRoomIndex = 1;
    public GameObject player;
    private bool objectPicked;
    private bool objectDestroyed;
    public bool willReturnToMuseum;
    public string song = "";

    // Start is called before the first frame update
    void Start()
    {
        objectPicked = false;
        objectDestroyed = false;
        sanityMeter = 10;
    }

    public void gameOver()
    {
        Debug.Log("GAME OVER!");
    }

    public string returnSong()
    {
        switch (nextRoomIndex)
        {
            case 1:
                song = "MusicBoxChildRoom";
                break;
            case 2:
                song = "Tade";
                break;
            default:
                break;
        }
        return song;
    }

    public void changeSanityLevel(int num)
    {
        sanityMeter += num;

        if (sanityMeter <= 0)
        {
            sanityMeter = 0;
            gameOver();
        }
        else if (sanityMeter > 20)
            sanityMeter = 20;
    }

    void returnToMuseum()
    {
        if (nextRoomIndex == 1)
        {
            if (objectDestroyed) nextRoomIndex = 2;
            else nextRoomIndex = 3;
        }

        objectPicked = false;
        objectDestroyed = false;
    }

    public Transform GetNextRoom()
    {
        if (!willReturnToMuseum)
        {
            Debug.Log("Tha paei allou");
            museumPlayerTransform.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            return teleportLocations[nextRoomIndex - 1];
        }
        else
        {
            Debug.Log("Tha girisei mouseio");
            returnToMuseum();
            return museumPlayerTransform;
        }
    }

    public void pickObject()
    {
        objectPicked = true;
    }

    public void DestroyObject()
    {
        objectDestroyed = true;
    }

    public void TeleportPlayer()
    {
        Debug.Log("TELEPORT!!");
        player.transform.position = GetNextRoom().position;
        willReturnToMuseum = !willReturnToMuseum;
    }

    public int GetSanity()
    {
        return sanityMeter;
    }


    // Update is called once per frame
    void Update()
    {
        if (sanityMeter < 5)
            Debug.Log("Kokkino");
        else if (sanityMeter < 7)
            Debug.Log("Orange");
        else if (sanityMeter < 10)
            Debug.Log("Yellow");
        else if (sanityMeter < 15)
            Debug.Log("Light Green");
        else
            Debug.Log("Dark Green");

    }
}
