using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.CK
{
    public class InteractableManager : MonoBehaviour
    {

        public enum InteractableType
        {
            Door,
            ShakingPainting,
            AudioPainting
        };


        public InteractableType TYPE;


        private void Start()
        {

        }


        public void DoorInteraction()
        {

        }

        //Shake
        public void InvokeShakeEffect()
        {
            if(GetComponent<CameraShake>())
            {
                GetComponent<CameraShake>().enabled = true;
            }
        }

        //Audio/General
        public void PlayAudio()
        {
            if(!gameObject.GetComponent<AudioSource>().isPlaying)
                gameObject.GetComponent<AudioSource>().Play();
        }

    }
}