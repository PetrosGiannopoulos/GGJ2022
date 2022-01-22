using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private int sanityMeter;

    // Start is called before the first frame update
    void Start()
    {
        sanityMeter = 50;
    }

    // Update is called once per frame
    void Update()
    {

        if (sanityMeter < 20)
            Debug.Log("Kokkino");
        else if (sanityMeter < 40)
            Debug.Log("Orange");
        else if (sanityMeter < 60)
            Debug.Log("Yellow");
        else if (sanityMeter < 80)
            Debug.Log("Light Green");
        else
            Debug.Log("Dark Green");

    }
}
