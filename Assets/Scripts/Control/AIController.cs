using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        float chaseDistance=5f;
        float supicionTime=3f;
        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer=Mathf.Infinity;
        float timeSinceArrivedAtWayPoint=Mathf.Infinity;
        [SerializeField] PathPatrol pathPatrol;
        float wayPointTolerance = 1f;
        float wayPointDwellTime = 3f;
        [Range(0,1)]
        float patrolSpeedFraction = 0.2f;
        int currentWaypointIndex=0;
        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition=transform.position;
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (InRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < supicionTime)
            {
                SupicionBehaviour();
            }
            else
            {
                PathPatrolBehaviour();
            }
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PathPatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (pathPatrol != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition,patrolSpeedFraction);

            }
        }

        private Vector3 GetCurrentWayPoint()
        {
            return pathPatrol.GetWayPoint(currentWaypointIndex);
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = pathPatrol.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SupicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;//
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}