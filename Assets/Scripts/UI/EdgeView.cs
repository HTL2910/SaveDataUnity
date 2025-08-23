// Assets/Scripts/View/EdgeView.cs
using UnityEngine;

public class EdgeView : MonoBehaviour
{
    public LineRenderer m_lr;
    public void Init(Vector3 a, Vector3 b)
    {
        m_lr.positionCount = 2;
        m_lr.SetPosition(0, a);
        m_lr.SetPosition(1, b);
    }
    public void UpdateB(Vector3 b) => m_lr.SetPosition(1, b);
}
