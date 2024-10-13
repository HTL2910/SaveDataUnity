using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class APathFinding : MonoBehaviour
{
    public float gCost;
    public float hCost;
    public float fCost=>gCost+hCost;

}
