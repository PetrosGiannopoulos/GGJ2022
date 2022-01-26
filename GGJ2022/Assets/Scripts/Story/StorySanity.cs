using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySanity : MonoBehaviour
{

    public static StorySanity instance;

    //Low Values < 0 == Insanity/Madness 
    //Hi Values > 0 == (Wisedom? or safe mind)
    int currentStorySanity=0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    //can be negative
    public void AddSanityPoints(int value)
    {
        currentStorySanity += value;
        Debug.Log($"CurrentSanityPoints: {currentStorySanity}");
    }

    public int GetStorySanity()
    {
        return currentStorySanity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
