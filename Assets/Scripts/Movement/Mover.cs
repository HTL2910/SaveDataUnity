using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Movement
{
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

            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
        }
        private void UpdateAnimator()
        {
            Vector3 veclocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(veclocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}