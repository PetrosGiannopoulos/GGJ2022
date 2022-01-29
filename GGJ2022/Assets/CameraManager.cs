using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public List<Camera> cameraList = new List<Camera>();

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

    public void DisableMainCamera()
    {
        Camera.main.gameObject.SetActive(false);
    }

    public void EnableCamera(string name)
    {
        DisableMainCamera();
        foreach(Camera cam in cameraList) {
            if (cam.gameObject.name.Equals(name))
            {
                cam.gameObject.SetActive(true);
                return;
            }
        }
    }

    public GameObject GetCameraObject(string name) {
        foreach (Camera cam in cameraList)
        {
            if (cam.gameObject.name.Equals(name))
            {
                
                return cam.gameObject;
            }
        }
        return null;
    }

}
