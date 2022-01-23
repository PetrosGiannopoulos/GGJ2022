using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform nextPortal;


    public Transform TeleportNext()
    {
        nextPortal.gameObject.SetActive(false);
        return nextPortal.transform;
    }

}
