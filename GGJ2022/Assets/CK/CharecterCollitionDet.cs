using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGJ.CK;

public class CharecterCollitionDet : MonoBehaviour
{
    public PlayerMovement pl;

    private void Awake()
    {
        pl = this.transform.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Portal")
        {
            Portal p = other.gameObject.GetComponent<Portal>();
            StartCoroutine(Telep(p));
        }
    }


    IEnumerator Telep(   Portal p)
    {
        pl.enabled = false;
        Transform t = p.TeleportNext();
        transform.position = t.position;


        //add fade effect or something
        yield return new WaitForSeconds(1f);

        pl.enabled = true;
    }
}
