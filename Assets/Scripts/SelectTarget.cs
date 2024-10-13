using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    public Material Mattarget;
    public Material Matstart;
    public GameObject target;
    public GameObject player;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                target = hit.collider.gameObject;
                target.GetComponent<MeshRenderer>().material= Mattarget;
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                player = hit.collider.gameObject;
                player.GetComponent<MeshRenderer>().material= Matstart;
            }
        }
    }
}
