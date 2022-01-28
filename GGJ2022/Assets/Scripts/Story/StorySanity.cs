using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySanity : MonoBehaviour
{

    public static StorySanity instance;
    public Image sanityImage;

    private int minSanity = 0;
    private int maxSanity = 100;

    
    //Low Values < 0 == Insanity/Madness 
    //Hi Values > 0 == (Wisedom? or safe mind)
    int currentStorySanity=50;
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
        //Debug.Log($"CurrentSanityPoints: {currentStorySanity}");

        sanityImage.fillAmount = GetNormalizedValue();
    }

    public float GetNormalizedValue()
    {
        //0-1
        return (currentStorySanity - ((float)minSanity)) / (((float)maxSanity) - ((float)minSanity));
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
