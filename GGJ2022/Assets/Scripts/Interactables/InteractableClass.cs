using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace GGJ.CK
{


    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class InteractableClass : MonoBehaviour
    {

        GameController gameController;
        DialogUI dialogUI;
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
            TELEPORT
        }
        public USE MyUse = USE.UNSPECIFIED;

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
            if(OnInteractionAction != null)
                OnInteractionAction.Invoke();
        }

        public void EventCaller()
        {
            foreach(UnityEvent e in eventsOnInteraction)
            {
                if(e != null)
                    e.Invoke();
            }
        }
        #endregion

        void UsageFlow()
        {
            switch(MyUse)
            {
                case USE.UNSPECIFIED:
                Debug.Log("#Interactable# Usage is unspecified !!");
                break;
                case USE.SINGLE:
                //Do nothing in here
                Debug.Log("#Interactable# Single Use !!");

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
                    gameController.TeleportPlayer(false);
                    break;
            }
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
            switch (parentObj.name)
            {
                case "Belt":
                    gameController.pickObject();
                    dialogUI.ClearDialogs();

                    List<string> dialogChoices = new List<string>();

                    
                    dialogChoices.Add("1) Yes. Blah blah blah");
                    dialogChoices.Add("2) No. Blah blah blah");

                    
                    //List<string> dialogChoices = gameController.GetDialogs();
                    foreach (string s in dialogChoices)
                    {
                        dialogUI.AddDialogChoice(s);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void PlayOneShot(AudioClip sfx)
        {
            if(sfx != null || inter_AudioSource != null)
            {
                if(inter_AudioSource.isPlaying)
                    inter_AudioSource.Stop();
                //then
                inter_AudioSource.clip = sfx;
                inter_AudioSource.Play();
            }

        }
    }

}