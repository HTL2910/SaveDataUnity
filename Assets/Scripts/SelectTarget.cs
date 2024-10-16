using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    public Material Mattarget;
    public Material Matstart;
    public Material Matdelete;
    public GameObject target;
    public GameObject player;
    public GameObject delete;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                delete = hit.collider.gameObject;
                delete.GetComponent<MeshRenderer>().material =Matdelete;
            }
        }
    }

}
