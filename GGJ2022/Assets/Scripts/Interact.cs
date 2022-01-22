using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("DoorMainTrigger"))
        {
            Debug.Log("EnterCollide");
            GameObject mainDoor = other.transform.parent.gameObject;
            Door[] doors = mainDoor.GetComponentsInChildren<Door>();
            
            foreach(Door door in doors)
            {
                GameObject doorObj = door.gameObject;
                StartCoroutine(OpenDoorGradually(doorObj,door.side));
                
            }

        }
    }

    IEnumerator OpenDoorGradually(GameObject door, int side)
    {
        yield return null;

        int length = 1;
        float time = 2f;
        float step = length/time;


        Debug.Log($"Side moving {side}");
        for(float i = 0; i < length; i += step)
        {
            door.transform.position += new Vector3(0,0,Time.deltaTime); 
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("DoorMainTrigger"))
        {
            GameObject mainDoor = other.transform.parent.gameObject;
            Door[] doors = mainDoor.GetComponentsInChildren<Door>();

            foreach (Door door in doors)
            {
                GameObject doorObj = door.gameObject;
                StartCoroutine(OpenDoorGradually(doorObj, -door.side));

            }
        }
    }
}
