using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGJ.CK;
public class Door: InteractableClass
{

    public int side=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnterInteraction()
    {
        base.OnEnterInteraction();

        Debug.Log("Door Interaction");


    }

    public override void OnExitInteraction()
    {
        base.OnExitInteraction();

        Debug.Log("Door Interaction");


    }
}
