using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public LayerMask bench;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetMouseButton(2))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 100, -Vector3.up, out hit, Mathf.Infinity,bench))
            {
                float mouseX = Input.GetAxis("Mouse X");
                transform.Rotate(0, 0, mouseX * 100 * Time.deltaTime);
            }
        }
    }
}
