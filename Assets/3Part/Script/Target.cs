using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform targetMarker;

    void Start()
    {
    }

    void Update()
    {
        int button = 0;

        //Get the point of the hit position when the mouse is being clicked
        //마우스를 클릭하면 충돌지점을 얻는다.
        
       
            if (Input.GetMouseButtonDown(button)) //if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                targetMarker.position = targetPosition;
            }
        }
    }

}