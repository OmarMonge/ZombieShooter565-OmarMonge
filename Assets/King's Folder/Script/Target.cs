using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Target : MonoBehaviour
{
    public Camera myCamera;
    public LayerMask playerLayer;

    public GameObject thisObject;

    private void Update()
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ~playerLayer)) // Use ~ to invert the layer mask
        {
            // Check if the hit object is on the player layer
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // If the hit object is the player, move the object along the ray's direction
                thisObject.transform.position = ray.origin + ray.direction * 100f; // Adjust 100f to desired distance
            }
            else
            {
                // If the ray hits an object not on the player layer, move the object to the hit point
                thisObject.transform.position = raycastHit.point;
            }
        }
        else
        {
            // If the ray doesn't hit anything, move the object along the ray's direction
            thisObject.transform.position = ray.origin + ray.direction * 100f; // Adjust 100f to desired distance
        }
    }
}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

//public class Target : MonoBehaviour
//{
//    public Camera myCamera;
//    public LayerMask player;

//    public GameObject thisObject;
//    private void Update()
//    {
//        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

//        if (Physics.Raycast(ray, out RaycastHit raycastHit))
//        {
//            thisObject.transform.position = raycastHit.point;
//        }
//        else
//        {
//            // If the ray doesn't hit anything, move the object along the ray's direction
//            thisObject.transform.position = ray.origin + ray.direction * 100f; // Adjust 100f to desired distance
//        }
//    }
//}
