// Assets/Scripts/View/EdgeView.cs
using UnityEngine;

public class EdgeView : MonoBehaviour
{
    public LineRenderer lr;
    public void Init(Vector3 a, Vector3 b)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
    }
    public void UpdateB(Vector3 b) => lr.SetPosition(1, b);
}
