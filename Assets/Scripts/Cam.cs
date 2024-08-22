using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    static Camera cam;
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(Camera.main.fieldOfView < 60)
        {
        Camera.main.fieldOfView -= scroll * 30;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 60);
        }
        else if(Camera.main.fieldOfView >= 60)
        {
            Camera.main.fieldOfView -= scroll * 20;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 59, 90);
        }
    }
}
