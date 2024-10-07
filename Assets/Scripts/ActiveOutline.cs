using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOutline : MonoBehaviour
{
    public void Active()
    {
        gameObject.GetComponent<Outline>().enabled = true;
    }
    public void Deactive()
    {
        gameObject.GetComponent<Outline>().enabled = false;

    }
}
