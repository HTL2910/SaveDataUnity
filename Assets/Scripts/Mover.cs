using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    //Ray lastRay;
    //private void Update() code move player to target and draw ray from camera to mouse position
    //{
    //    GetComponent<NavMeshAgent>().destination = target.position;
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        lastRay=Camera.main.ScreenPointToRay(Input.mousePosition);
    //    }    
    //    Debug.DrawRay(lastRay.origin, lastRay.direction*100);
    //}

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }    
    }
    private void MoveToCursor()
    {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hashit=Physics.Raycast(ray, out hit);
        if(hashit)
        {
            GetComponent<NavMeshAgent>().destination=hit.point;
        }
    }    
}
