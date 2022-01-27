using GGJ.CK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum ENDING
    {
        GOODENDING1,
        GOODENDING2,
        BADENDING1,
        BADENDING2,
    }

    public ENDING gameEnding = ENDING.BADENDING2;
    private int sanityMeter;
    public List<Transform> teleportLocations = new List<Transform>();
    public Transform museumPlayerTransform;
    private int nextRoomIndex = 1;
    public GameObject player;
    private bool objectPicked;
    private bool objectDestroyed;
    public bool willReturnToMuseum;
    public string[] songs;
    public bool secondRoomIsGood = true;
    public bool thirdRoomIsGood = false;
    public bool thirdRoomIsNeutral = false;
    public GameObject playerHud;

    // Start is called before the first frame update
    void Start()
    {
        objectPicked = false;
        objectDestroyed = false;
        sanityMeter = 10;

        StartCoroutine(delayTransfer());
        
    }

    IEnumerator delayTransfer()
    {
        yield return new WaitForSeconds(2);
        //player.transform.position = LocationManager.instance.GetLocationPos("BadEnding2");

    }

    public void gameOver()
    {
        Debug.Log("GAME OVER!");
    }

    //public string returnSong()
    //{
    //    switch (nextRoomIndex)
    //    {
    //        case 1:
    //            song = "MusicBoxChildRoom";
    //            break;
    //        case 2:
    //            song = "Tade";
    //            break;
    //        default:
    //            break;
    //    }
    //    return song;
    //}

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

    public void TeleportPlayer(int num)
    {
        Debug.Log("TELEPORT!!");
        //player.transform.position = GetNextRoom().position;
        if (num == 6)
        {
            if (LocationManager.instance.defaultRoom1) StorySanity.instance.AddSanityPoints(-1);
            //Exiting Room 1
            if (StorySanity.instance.GetStorySanity() >=0)
            {
                //go to Clean Room
                teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointRoom2");
            }
            else
            {
                //go to Messy Room
                teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointRoom3");
            }

        }
        else if (num == 7)
        {

            teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointGarage");

            if (StorySanity.instance.GetStorySanity() >= 0)
            {
                //garage good scenario
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario
                GarageBadScenario();
            }
        }
        else if (num == 8)
        {
            teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointGarage");

            if (StorySanity.instance.GetStorySanity() >= 0)
            {
                //garage good scenario
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario
                GarageBadScenario();
            }
        }


        player.transform.position = teleportLocations[num].position;
        willReturnToMuseum = !willReturnToMuseum;
    }

    public void GarageGoodScenario()
    {
        GameObject[] garageItems = GameObject.FindGameObjectsWithTag("GarageItems");

        foreach(GameObject garageItem in garageItems)
        {
            string parentName = garageItem.transform.parent.gameObject.name;
            switch (parentName)
            {
                case "Drugs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 1;
                    break;
                case "Hand-Cuffs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 1;
                    break;
                case "Doll":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 1;
                    break;
                case "MoneyBriefcase":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -1;
                    break;
                case "DeadBodyCovered":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 0;

                    garageItem.GetComponent<InteractableClass>().dialogChoices.Clear();
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Do nothing.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Pickpocket and Hide.");
                    break;
                default:
                    break;
            }
        }

       

    }

    public void GarageBadScenario()
    {
        GameObject[] garageItems = GameObject.FindGameObjectsWithTag("GarageItem");

        foreach (GameObject garageItem in garageItems)
        {
            string parentName = garageItem.transform.parent.gameObject.name;
            switch (parentName)
            {
                case "Drugs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -1;
                    break;
                case "Hand-Cuffs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -1;
                    break;
                case "Doll":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -1;
                    break;
                case "MoneyBriefcase":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 1;
                    break;
                case "DeadBodyCovered":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 0;

                    garageItem.GetComponent<InteractableClass>().dialogChoices.Clear();
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Report to police.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Do nothing.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Pickpocket.");
                    break;
                default:
                    break;
            }
        }
    }

    public int GetSanity()
    {
        return sanityMeter;
    }


    // Update is called once per frame
    void Update()
    {
        /*
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
        */
    }
}
