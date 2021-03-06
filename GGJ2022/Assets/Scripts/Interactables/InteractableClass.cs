using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace GGJ.CK
{


    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class InteractableClass : MonoBehaviour
    {

        GameController gameController;
        public DialogUI dialogUI;
        
        public enum TYPE
        {
            UNSPECIFIED,
            KEY,
            DOOR,
            ARTIFACT,
            COLLECTOR,
            PAPPER,
            TORCHE,
            TELEPORTATION
        }
        public TYPE myType = TYPE.UNSPECIFIED;

        /// <summary>
        /// Single use : Door -> open/close
        /// Collect : Key -> store to inventory
        /// Equip : Torche -> equip to player
        /// </summary>
        public enum USE
        {
            UNSPECIFIED,
            SINGLE,
            COLLECT,
            EQUIP,
            READ,
            DESTROY,
            OPEN,
            CLOSE,
            THROW,
            TELEPORT,
            END
        }
        public USE MyUse = USE.UNSPECIFIED;
        public List<string> dialogChoices = new List<string>();
        public string itemName;
        public int sanityModifier;
        public bool willReturnToMuseum;
        public LayerMask interactableLayerMask;
        [Space]
        //Action
        private Action OnInteractionAction;
        //private void AddListener() => OnInteractionAction += ItemFuctionallity;
        //private void RemoveListener() => OnInteractionAction -= ItemFuctionallity;

        public List<UnityEvent> eventsOnInteraction;

        //my audio
        protected AudioSource inter_AudioSource;




        private void Awake()
        {
            //gameObject.layer = itemLayer;
        }

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            dialogUI = GameObject.FindGameObjectWithTag("DialogUI").GetComponent<DialogUI>();
        }

        #region DefaultMethods
        void Init()
        {
            inter_AudioSource = GetComponent<AudioSource>();
            //AddListener();


        }

        void OnEnable()
        {
            Debug.Log("Action ITem");
            Init();
        }

        void OnDisable()
        {
            //RemoveListener();
        }
        #endregion

        #region Delegates_Actions_Events
        public void ActionCaller()
        {
            if (OnInteractionAction != null)
                OnInteractionAction.Invoke();
        }

        public void EventCaller()
        {
            foreach (UnityEvent e in eventsOnInteraction)
            {
                if (e != null)
                    e.Invoke();
            }
        }
        #endregion

        void UsageFlow()
        {
            switch (MyUse)
            {
                case USE.UNSPECIFIED:
                    Debug.Log("#Interactable# Usage is unspecified !!");
                    break;
                case USE.SINGLE:
                    //Do nothing in here
                    Debug.Log("#Interactable# Single Use !!");
                    
                    AudioManager.instance.StopCurrent();
                    if (StorySanity.instance.GetStorySanity() >= 50)
                    {
                        AudioManager.instance.PlayFadeIn("HerculesGood");
                        TextRendererManager.instance.InitHerculesStatueGoodText();
                    }
                    else
                    {
                        AudioManager.instance.PlayFadeIn("HerculesBad");
                        TextRendererManager.instance.InitHerculesStatueBadText();
                    }

                    gameController.blockingPuzzle.SetActive(false);
                    gameController.blockingStatue.SetActive(true);
                    Destroy(gameObject, 0.1f);
                    break;
                case USE.COLLECT:
                    Debug.Log("#Interactable# collect Use !!");

                    break;
                case USE.EQUIP:
                    Debug.Log("#Interactable# equip Use !!");
                    //Debug.Log(gameObject.name);
                    PickupObject();
                    break;

                case USE.READ:
                    Debug.Log("#Interactable# read Use !!");
                    break;
                case USE.TELEPORT:
                    int num = 0;
                    switch (itemName)
                    {
                        case "Paint1":
                            num = 0;
                            break;
                        case "Paint2":
                            num = gameController.secondRoomIsGood ? 1 : 2;
                            break;
                        case "Paint3":
                            num = gameController.thirdRoomIsNeutral ? 5 : gameController.thirdRoomIsGood ? 3 : 4;
                            break;
                        case "Room1Door":
                            num = 6;
                            break;
                        case "Room21Door":
                            num = 7;
                            break;
                        case "Room22Door":
                            num = 8;
                            break;
                        case "Elevator1":
                            num = 9;
                            break;
                        case "DarkEnter":
                            Debug.Log("EnteringDarkPainting");
                            num = 10;
                            break;
                        case "Portal":
                            num = 13;
                            break;
                        default:
                            break;
                    }

                    /*if (!willReturnToMuseum)
                    {
                        AudioManager.instance.Stop("MainTheme");
                        AudioManager.instance.PlayFadeIn(gameController.songs[num]);
                    }
                    else
                    {
                        AudioManager.instance.Stop(gameController.songs[num]);
                        AudioManager.instance.PlayFadeIn("MainTheme");
                    }*/
                    //gameController.TeleportPlayer(num);
                    gameController.TeleportPlayer(itemName);
                    Destroy(this);
                    if (gameObject.tag.Equals("Portal")) StartCoroutine(DestroySelf());
                    break;
                case USE.END:
                    switch (gameController.gameEnding)
                    {
                        case GameController.ENDING.GOODENDING1:
                            Debug.Log("GOOD ENDING 1 END GAME !!");
                            CameraManager.instance.EnableCamera("GoodEnding1Camera");
                            GameObject.Find("TherapistGuy").GetComponent<Animator>().Play("Talking");
                            GameObject.Find("LyingDownGuy").GetComponent<Animator>().Play("Sit");
                            
                            StartCoroutine(AnimationDoF(2f,100f,1));
                            break;
                        case GameController.ENDING.GOODENDING2:
                            Debug.Log("GOOD ENDING 2 END GAME !!");
                            CameraManager.instance.EnableCamera("GoodEnding2Camera");
                            CameraManager.instance.GetCameraObject("GoodEnding2Camera").GetComponent<Animator>().Play("GoodEnding2CameraAnimation");
                            GameObject.Find("WakeUpGuy").GetComponent<Animator>().Play("WakeUp");
                            StartCoroutine(AnimationDoF(4f,100f,2));
                            break;
                        case GameController.ENDING.BADENDING1:
                            Debug.Log("BAD ENDING 1 END GAME !!");
                            CameraManager.instance.EnableCamera("BadEnding1Camera");
                            StartCoroutine(AnimationDoF(2f,100f,3));

                            break;
                        case GameController.ENDING.BADENDING2:
                            CameraManager.instance.EnableCamera("BadEnding2Camera");

                            //Start falling to death
                            GameObject fallingGuy = GameObject.Find("FallingGuy");
                            fallingGuy.GetComponent<Rigidbody>().isKinematic = false;
                            //StartCoroutine(DelayFall(0.6f));
                            StartCoroutine(AnimationDoF(2f,100f,4));

                            break;
                    }
                    gameController.playerHud.SetActive(false);
                    break;
            }
        }

        IEnumerator AnimationDoF(float delayTime, float endValue, int endingNum)
        {

            yield return new WaitForSeconds(delayTime);

            VolumeProfile profile = GameController.instance.volume.sharedProfile;
            DepthOfField dof;
            profile.TryGet(out dof);

            float startValue = 1f;
            //float endValue = 41.7f;
            //float endValue = 100f;
            float step = 0.4f;

            dof.focalLength.value = startValue;
            
            float value = dof.focalLength.value;

            while (value < endValue)
            {
                value += step;
                dof.focalLength.value = value;
                yield return new WaitForSeconds(0.02f);
            }

            switch (endingNum)
            {
                case 1:
                    AudioManager.instance.StopCurrent();
                    AudioManager.instance.PlayFadeIn("Ending1");
                    TextRendererManager.instance.SetEndingText("Ending1",30);
                    break;
                case 2:
                    AudioManager.instance.StopCurrent();
                    AudioManager.instance.PlayFadeIn("Ending2");
                    TextRendererManager.instance.SetEndingText("Ending2",30);
                    break;
                case 3:
                    AudioManager.instance.StopCurrent();
                    AudioManager.instance.PlayFadeIn("Ending3");
                    TextRendererManager.instance.SetEndingText("Ending3",30);
                    break;
                case 4:
                    AudioManager.instance.StopCurrent();
                    AudioManager.instance.PlayFadeIn("Ending4");
                    TextRendererManager.instance.SetEndingText("Ending4",40);
                    break;
                default:
                    break;
            }
        }

        IEnumerator DelayFall(float delayTime)
        {

            GameObject fallingGuy = GameObject.Find("FallingGuy");
            fallingGuy.GetComponent<Animator>().Play("Falling");
            yield return new WaitForSeconds(delayTime);

            fallingGuy.GetComponent<Animator>().Play("FallingDown");
            
        }

        IEnumerator DestroySelf()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }


        /// <summary>
        /// The fuctions below share the same logic with all interactable objs
        /// </summary>
        virtual public void OnInteraction()
        {
            ActionCaller();
            EventCaller();
            UsageFlow();
            //Debug.Log("#ItemBaseClass#Parrent call listener");
        }

        virtual public void OnEnterInteraction()
        {

        }

        virtual public void OnExitInteraction()
        {

        }

        public void PickupObject()
        {
            GameObject parentObj = gameObject.transform.parent.gameObject;
            dialogUI.ClearDialogs();

            Debug.Log($"Item Name: {parentObj.name}, dialogChoicesCount: {dialogChoices.Count}");
            foreach (string s in dialogChoices) dialogUI.AddDialogChoice(s, itemName, dialogChoices.Count);

            dialogUI.ResetKeyState();
            StorySanity.instance.AddSanityPoints(sanityModifier);

            AddDialogText();

            if (parentObj.name.Equals("TV"))
            {
                GameObject.Find("Room22Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
                GameObject tvImage = GameObject.Find("TVImage");
                tvImage.GetComponent<TVAnimation>().InitPlayback();
            }
            else if (parentObj.name.Equals("Teddy_Bear"))
            {
                Debug.Log("Room1 Unlocking");
                LocationManager.instance.room1Unlocked = true;
                GameObject.Find("Room1Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else if (parentObj.name.Equals("PhoneClean"))
            {
                LocationManager.instance.room21Unlocked = true;
                GameObject.Find("Room21Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else if (parentObj.name.Equals("DeadBodyCovered"))
            {
                LocationManager.instance.garageRoomUnlocked = true;
            }

            if(sanityModifier!=0)Destroy(gameObject, 0.1f);

            /*
            switch (parentObj.name)
            {
                case "Belt":
                    //gameController.pickObject();
                    dialogUI.ClearDialogs();

                    dialogChoices.Add("1) Throw it away");
                    dialogChoices.Add("2) No. Keep it.");

                    //List<string> dialogChoices = gameController.GetDialogs();
                    foreach (string s in dialogChoices)
                    {
                        dialogUI.AddDialogChoice(s, itemName);
                    }

                    dialogUI.ResetKeyState();
                    break;
                case "PhoneClean":
                    gameController.pickObject();
                    dialogUI.ClearDialogs();

                    dialogChoices.Add("1) Answer to message.");
                    dialogChoices.Add("2) Ignore message.");

                    foreach (string s in dialogChoices)
                    {
                        dialogUI.AddDialogChoice(s, itemName);
                    }
                    dialogUI.ResetKeyState();

                    LocationManager.instance.room21Unlocked = true;
                    GameObject.Find("Room21Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;

                    break;
                case "Toy_Train":

                    StorySanity.instance.AddSanityPoints(+1);
                    /*GameObject pickupTrigger = null;
                    for(int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        GameObject childGO = gameObject.transform.GetChild(i).gameObject;
                        if (childGO.name.Equals("PickupTrigger")) pickupTrigger = childGO;
                    }
                    Destroy(gameObject, 0.1f);
                    //Destroy(pickupTrigger,0.1f);
                    break;
                case "Teddy_Bear":
                    Debug.Log("Room1 Unlocking");
                    LocationManager.instance.room1Unlocked = true;
                    GameObject.Find("Room1Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    

                    break;
                case "Desk":
                    //Child Room Globe
                    StorySanity.instance.AddSanityPoints(+1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "DeskGlobe":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "GuitarClean":
                    StorySanity.instance.AddSanityPoints(+1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "WindowClean":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "PhoneMessy":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "tv":
                    GameObject.Find("Room22Door").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
                    StorySanity.instance.AddSanityPoints(+1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "WindowMessy":
                    StorySanity.instance.AddSanityPoints(+1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "Carpet":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "GuitarMessy":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "DrugsClean":
                    StorySanity.instance.AddSanityPoints(+1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "DrugsMessy":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameObject, 0.1f);
                    break;
                case "RecordPlayerClean":
                    StorySanity.instance.AddSanityPoints(-1);
                    Destroy(gameController,0.1f);
                    break;
                default:
                    break;
                    
            }
            */
        }

        public void AddDialogText()
        {
            GameObject parentObj = gameObject.transform.parent.gameObject;

            TextRendererManager.instance.SetPickupText(parentObj.name);
            
            if(parentObj.name.Equals("DeadBodyCovered") && dialogChoices.Count == 3)
            {
                TextRendererManager.instance.SetPickupText("DeadBodyCovered31");
            }
            else if(parentObj.name.Equals("DeadBodyCovered") && dialogChoices.Count == 4)
            {
                TextRendererManager.instance.SetPickupText("DeadBodyCovered32");
            }

            if (parentObj.name.Equals("Doll"))
            {
                //GameObject deadbody = GameObject.Find("DeadBodyCovered");
                //if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 3)
                //{
                //    TextRendererManager.instance.SetPickupText("Doll31");
                //}
                //else if(deadbody.GetComponent<InteractableClass>().dialogChoices.Count==4)
                //{
                //    TextRendererManager.instance.SetPickupText("Doll32");
                //}

                if (GameController.instance.garageIsGood)
                {
                    TextRendererManager.instance.SetPickupText("Doll31");
                }
                else
                {
                    TextRendererManager.instance.SetPickupText("Doll32");
                }
            }

            if (parentObj.name.Equals("Drugs"))
            {
                //GameObject deadbody = GameObject.Find("DeadBodyCovered");
                //if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 3)
                //{
                //    TextRendererManager.instance.SetPickupText("Drugs31");
                //}
                //else if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 4)
                //{
                //    TextRendererManager.instance.SetPickupText("Drugs32");
                //}

                if (GameController.instance.garageIsGood)
                {
                    TextRendererManager.instance.SetPickupText("Drugs31");
                }
                else
                {
                    TextRendererManager.instance.SetPickupText("Drugs32");
                }
            }

            if (parentObj.name.Equals("Hand-Cuffs"))
            {
                //GameObject deadbody = GameObject.Find("DeadBodyCovered");
                //if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 3)
                //{
                //    TextRendererManager.instance.SetPickupText("HandCuffs31");
                //}
                //else if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 4)
                //{
                //    TextRendererManager.instance.SetPickupText("HandCuffs32");
                //}

                if (GameController.instance.garageIsGood)
                {
                    TextRendererManager.instance.SetPickupText("HandCuffs31");
                }
                else
                {
                    TextRendererManager.instance.SetPickupText("HandCuffs32");
                }
            }

            if (parentObj.name.Equals("MoneyBriefcase"))
            {
                //GameObject deadbody = GameObject.Find("DeadBodyCovered");
                //if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 3)
                //{
                //    TextRendererManager.instance.SetPickupText("MoneyBriefcase31");
                //}
                //else if (deadbody.GetComponent<InteractableClass>().dialogChoices.Count == 4)
                //{
                //    TextRendererManager.instance.SetPickupText("MoneyBriefcase32");
                //}

                
                if (GameController.instance.garageIsGood)
                {
                    TextRendererManager.instance.SetPickupText("MoneyBriefcase31");
                }
                else
                {
                    TextRendererManager.instance.SetPickupText("MoneyBriefcase32");
                }
            }

        }
        protected void PlayOneShot(AudioClip sfx)
        {
            if (sfx != null || inter_AudioSource != null)
            {
                if (inter_AudioSource.isPlaying)
                    inter_AudioSource.Stop();
                //then
                inter_AudioSource.clip = sfx;
                inter_AudioSource.Play();
            }

        }
    }

}