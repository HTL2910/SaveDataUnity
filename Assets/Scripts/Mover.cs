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
        if(Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        UpdateAnimator();
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
    private void UpdateAnimator()
    {
        Vector3 veclocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(veclocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed) ;
    }
}
