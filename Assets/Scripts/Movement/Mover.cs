using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        NavMeshAgent navMeshAgent;
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
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {

            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 veclocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(veclocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}