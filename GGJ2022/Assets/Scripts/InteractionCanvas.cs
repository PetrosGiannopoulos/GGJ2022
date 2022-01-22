﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GGJ.CK{

public class InteractionCanvas : MonoBehaviour
{
    private PlayerMovement player;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        transform.LookAt(player.transform , Vector3.up);
    }


}

}