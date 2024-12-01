using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPatrol : MonoBehaviour
{
    const float waypointGizmoRadius = 0.3f;

    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(GetWayPoint(i), waypointGizmoRadius);
            Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
        }
    }

    public Vector3 GetWayPoint(int i)
    {
        return transform.GetChild(i).position;
    }

    public int GetNextIndex(int i)
    {
        if(i + 1 == transform.childCount)
        {
            return 0;
        }
        return i + 1;
    }
}
