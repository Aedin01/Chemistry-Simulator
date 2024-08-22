using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    public LayerMask groundLayer;
    public Rigidbody body;
    void Start()
    {
        mainCamera = Camera.main;
    }
    void OnMouseDown()
    {
        isDragging = true;
        
        if(body != null)
        {
            body.isKinematic = true;
        }

        offset = mainCamera.WorldToScreenPoint(transform.position) - Input.mousePosition;
        offset.z = mainCamera.WorldToScreenPoint(transform.position).z;
    }
    void OnMouseUp()
    {
        isDragging = false;
        if(body != null)
        {
            body.isKinematic = false;
        }
    }
    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.y = transform.position.y; 
            if(worldPosition.z > 8)
            {
                worldPosition.z = 8f;
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 2, -Vector3.up, out hit, Mathf.Infinity,groundLayer))
            {
                Vector3 newPosition = transform.position;
                worldPosition.y = hit.point.y; 
                transform.position = worldPosition;
            }

        }
    }
}
