using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    public static LocationManager instance;
    public List<GameObject> locations = new List<GameObject>();

    public bool defaultRoom1 = true;
    public bool defaultRoom21;
    public bool defaultRoom22;
    public bool defaultRoom31;
    public bool defaultRoom32;

    public bool room1Unlocked = false;
    public bool room21Unlocked = false;
    public bool room22Unlocked = false;
    public bool room31Unlocked = false;
    public bool room32Unlocked = false;
    public bool garageRoomUnlocked = false;
    

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

    

    public Vector3 GetLocationPos(string name)
    {
        foreach(GameObject go in locations)
        {
            if (go.name.Equals(name)) return go.transform.position;
        }
        return new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
