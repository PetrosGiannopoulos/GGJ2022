using GGJ.CK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public Volume volume;
    public enum ENDING
    {
        GOODENDING1,
        GOODENDING2,
        BADENDING1,
        BADENDING2,
    }

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

    public List<Material> endingMaterials = new List<Material>();
    public MeshRenderer endingRenderer;

    // Start is called before the first frame update
    void Start()
    {
        objectPicked = false;
        objectDestroyed = false;
        sanityMeter = 50;

        StartCoroutine(delayTransfer());

        VolumeProfile profile = GameController.instance.volume.sharedProfile;
        DepthOfField dof;
        profile.TryGet(out dof);

        dof.focalLength.value = 1f;
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
        Debug.Log($"TELEPORT!!Number: {num}");
        //player.transform.position = GetNextRoom().position;
        if (num == 6)
        {
            if (LocationManager.instance.defaultRoom1) StorySanity.instance.AddSanityPoints(-1);
            //Exiting Room 1
            if (StorySanity.instance.GetStorySanity() >45)
            {
                //go to Clean Room 2.1
                teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointRoom2");
            }
            else
            {
                //go to Messy Room 2.2 <=45
                teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointRoom3");
            }

        }
        else if (num == 7)
        {
            //From 2.1
            teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointGarage");

            if (StorySanity.instance.GetStorySanity() >=60)
            {
                //garage good scenario 3.1
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario 3.2 <60
                GarageBadScenario();
            }
        }
        else if (num == 8)
        {
            //From 2.2
            teleportLocations[num].position = LocationManager.instance.GetLocationPos("SpawnPointGarage");
            //GameObject tvImage = GameObject.Find("TVImage");
            //tvImage.GetComponent<TVAnimation>().StopPlayback();
            if (StorySanity.instance.GetStorySanity() >= 48)
            {
                //garage good scenario 3.1
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario 3.2 <48
                GarageBadScenario();
            }
        }
        else if (num == 12)
        {
            Debug.Log("TheaterPositionSpawn");
            //Go to Room 4
            teleportLocations[num].position = LocationManager.instance.GetLocationPos("TheaterPosition");
        }
        else if (num == 13)
        {
            //Go to Dark Room Ending

            float value = StorySanity.instance.GetStorySanity();
            if (value >= 75)
            {
                endingRenderer.material = endingMaterials[0];
            }
            else if (value >= 50)
            {
                endingRenderer.material = endingMaterials[1];
            }
            else if(value >= 25)
            {
                endingRenderer.material = endingMaterials[2];
            }
            else
            {
                endingRenderer.material = endingMaterials[3];
            }
            
            teleportLocations[num].position = LocationManager.instance.GetLocationPos("DarknessRoomSpawnPoint");
            Debug.Log("DarkRoomTeleport");
        }


        player.transform.position = teleportLocations[num].position;
        //willReturnToMuseum = !willReturnToMuseum;
    }

    public void TeleportPlayer(string name)
    {
        if (name.Equals("Room1Door"))
        {
            if (LocationManager.instance.defaultRoom1) StorySanity.instance.AddSanityPoints(-1);
            //Exiting Room 1
            if (StorySanity.instance.GetStorySanity() > 45)
            {
                //go to Clean Room 2.1
                player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointRoom2");
            }
            else
            {
                //go to Messy Room 2.2 <=45
                player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointRoom3");
            }
        }
        else if (name.Equals("Room21Door"))
        {
            player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointGarage");

            if (StorySanity.instance.GetStorySanity() >= 60)
            {
                //garage good scenario 3.1
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario 3.2 <60
                GarageBadScenario();
            }
        }
        else if (name.Equals("Room22Door"))
        {
            //From 2.2
            player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointGarage");
            GameObject tvImage = GameObject.Find("TVImage");
            tvImage.GetComponent<TVAnimation>().StopPlayback();
            if (StorySanity.instance.GetStorySanity() >= 48)
            {
                //garage good scenario 3.1
                GarageGoodScenario();
            }
            else
            {
                //garage bad scenario 3.2 <48
                GarageBadScenario();
            }
        }
        else if (name.Equals("Theater"))
        {
            player.transform.position = LocationManager.instance.GetLocationPos("TheaterPosition");
        }
        else if (name.Equals("Portal"))
        {
            float value = StorySanity.instance.GetStorySanity();
            if (value >= 75)
            {
                endingRenderer.material = endingMaterials[0];
                gameEnding = ENDING.GOODENDING1;
            }
            else if (value >= 50)
            {
                endingRenderer.material = endingMaterials[1];
                gameEnding = ENDING.GOODENDING2;
            }
            else if (value >= 25)
            {
                endingRenderer.material = endingMaterials[2];
                gameEnding = ENDING.BADENDING1;
            }
            else
            {
                endingRenderer.material = endingMaterials[3];
                gameEnding = ENDING.BADENDING2;
            }

            player.transform.position = LocationManager.instance.GetLocationPos("DarknessRoomSpawnPoint");
        }
        else if (name.Equals("Paint1"))
        {
            player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointRoom1");
        }
        else if (name.Equals("Paint2"))
        {
            player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointRoom2");
        }
        else if (name.Equals("Paint3"))
        {
            player.transform.position = LocationManager.instance.GetLocationPos("SpawnPointRoom3");
        }
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
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 5;
                    break;
                case "Hand-Cuffs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 5;
                    break;
                case "Doll":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 5;
                    break;
                case "MoneyBriefcase":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -5;
                    break;
                case "DeadBodyCovered":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 0;

                    garageItem.GetComponent<InteractableClass>().dialogChoices.Clear();
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Do nothing.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Pickpocket and Hide.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Keep Searching.");

                    break;
                default:
                    break;
            }
        }

       

    }

    public void GarageBadScenario()
    {
        GameObject[] garageItems = GameObject.FindGameObjectsWithTag("GarageItems");

        foreach (GameObject garageItem in garageItems)
        {
            string parentName = garageItem.transform.parent.gameObject.name;
            switch (parentName)
            {
                case "Drugs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -5;
                    break;
                case "Hand-Cuffs":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -5;
                    break;
                case "Doll":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = -5;
                    break;
                case "MoneyBriefcase":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 5;
                    break;
                case "DeadBodyCovered":
                    garageItem.GetComponent<InteractableClass>().sanityModifier = 0;

                    garageItem.GetComponent<InteractableClass>().dialogChoices.Clear();
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Report to police.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Do nothing.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Pickpocket.");
                    garageItem.GetComponent<InteractableClass>().dialogChoices.Add("Keep Searching.");

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
