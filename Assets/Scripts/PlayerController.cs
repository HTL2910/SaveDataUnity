using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{


    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }
        private void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hashit = Physics.Raycast(ray, out hit);
            if (hashit)
            {
                GetComponent<Mover>().MoveTo(hit.point);
            }
        }
    }
}