using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGJ.CK;

public class CharecterCollitionDet : MonoBehaviour
{
    public PlayerMovement pl;
    public PlayerRaycasting rc;
    public CanvasGroup gp;

    private void Awake()
    {
        pl = this.transform.GetComponent<PlayerMovement>();
        rc = FindObjectOfType<PlayerRaycasting>();

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
        rc.enabled = false;

        //add fade effect or something
        while(gp.alpha <= 0.9)
        {
 

            gp.alpha += 2f * Time.deltaTime;
            yield return new WaitForSeconds(0.025f);
        }

        Transform t = p.TeleportNext();
        transform.position = t.position;

        yield return new WaitForSeconds(0.25f);

        gp.alpha = 1;
        while(gp.alpha >= 0.1)
        {
            gp.alpha -= 4f * Time.deltaTime;
            yield return new WaitForSeconds(0.025f);
        }
        gp.alpha = 0;

        pl.enabled = true;
        rc.enabled = true;
    }



    //trash


    float de = 0;
}
