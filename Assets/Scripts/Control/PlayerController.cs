using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        void Update()
        {
            if(InteractWithCombat())return;
            if(InteractWithMovement())return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target=hit.transform.GetComponent<CombatTarget>();
                if(target == null) { continue; }
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (hashit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().MoveTo(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}