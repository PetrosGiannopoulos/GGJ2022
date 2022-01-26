using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    public static LocationManager instance;
    public List<GameObject> locations = new List<GameObject>();

    public int defaultRoom1 = -1;
    public int defaultRoom21;
    public int defaultRoom22;
    public int defaultRoom31;
    public int defaultRoom32;

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
