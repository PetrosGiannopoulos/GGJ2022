using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GGJ.CK
{

    public class PlayerMovement : MonoBehaviour
    {

        private CharacterController _controller;

        [SerializeField] private float _speed = 6f;
        [SerializeField] float _gravity = -9.81f;
        [Space]

        [SerializeField] private Transform _gCheck;
        [SerializeField] private float _gDistancel;
        [SerializeField] private LayerMask _gMask;
        [SerializeField] private LayerMask _areaMask;

        Vector3 velocity;
        [SerializeField] bool _isGrounded;
        [SerializeField] bool _isStepping;
        public Transform startPos;
        AudioSource audioStep;

        bool isWalking = false;

        Collider currentCollider;
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            audioStep = GetComponent<AudioSource>();
        }


        void Start()
        {
            //transform.position = startPos.position;

            //FindObjectOfType<PlayerMovement>().enabled = false;
            
        }

        void Update()
        {
            //Ground Check
            _isGrounded = Physics.CheckSphere(_gCheck.position , _gDistancel , _gMask);

            bool wasStepping = _isStepping;

            Collider[] stepColliders = Physics.OverlapSphere(_gCheck.position, _gDistancel, _areaMask);
            


            if (stepColliders.Length > 0) _isStepping = true;
            else _isStepping = false;

            if (_isStepping)
            {
                
                if (wasStepping == false)
                {
                    //entered interact area
                    Debug.Log("Stepping ON");
                    foreach(Collider collider in stepColliders)
                    {

                        if (collider.gameObject.tag.Equals("DoorMainTrigger"))
                        {
                            collider.gameObject.transform.parent.gameObject.GetComponent<Animator>().Play("OpenDoor");
                            currentCollider = collider;
                        }
                        
                    }
                }
            }
            else
            {
                if (wasStepping)
                {
                    //exit interact area
                    Debug.Log("Exit Area");

                   if (currentCollider.gameObject.tag.Equals("DoorMainTrigger")){
                            Debug.Log("Close Door");
                            currentCollider.gameObject.transform.parent.gameObject.GetComponent<Animator>().Play("CloseDoor");
                       
                        // collider.gameObject.GetComponent<InteractableClass>().OnExitInteraction();

                    }
                   
                }
            }

            if (_isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //Input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");


            //Movement
            Vector3 move = transform.right * x + transform.forward * z;
            _controller.Move(move * _speed * Time.deltaTime);

            velocity.y += _gravity * Time.deltaTime;
            _controller.Move(velocity * Time.deltaTime);

            if (_controller.velocity.magnitude > 0.01f && !audioStep.isPlaying)
            {
                /*audioStep.volume = Random.Range(0.6f, 1.0f);
                audioStep.pitch = Random.Range(0.8f, 1.0f);
                audioStep.Play();*/
            }

            if (move.magnitude < 0.1f)
            {
                isWalking = false;
                AudioManager.instance.AbruptStop("LeftFoot");
            }
            else
            {
                if (isWalking == false)
                {
                    isWalking = true;
                    AudioManager.instance.Play("LeftFoot");
                    
                }
            }

        }


        private void OnTriggerEnter(Collider other)
        {


            if(other.gameObject.tag == "Trap")
            {
                //Dead

                GameManager.Instance.OnDeath();
                //transform.position = startPos.position;
            }
            if(other.gameObject.tag == "End")
            {
                //end
                GameManager.Instance.OnWin();
                //transform.position = startPos.position;
            }

        }


        private void OnTriggerExit(Collider other)
        {

        }

    }

}