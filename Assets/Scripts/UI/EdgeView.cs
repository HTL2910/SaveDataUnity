using UnityEngine;

public class EdgeView : MonoBehaviour
{
    public LineRenderer m_lr;

    private NodeView m_a, m_b;

    /// <summary>
    /// Khởi tạo line giữa 2 vị trí tạm thời (khi drag).
    /// </summary>
    public void Init(Vector3 a, Vector3 b)
    {
        if (m_lr == null) return;
        m_lr.positionCount = 2;
        m_lr.SetPosition(0, a);
        m_lr.SetPosition(1, b);
    }

    /// <summary>
    /// Gắn node A và B, rồi vẽ line.
    /// </summary>
    public void SetNodes(NodeView a, NodeView b)
    {
        m_a = a;
        m_b = b;
        Refresh();
    }

    /// <summary>
    /// Refresh line theo vị trí node.
    /// </summary>
    public void Refresh()
    {
        if (m_lr != null && m_a != null && m_b != null)
        {
            m_lr.positionCount = 2;
            m_lr.SetPosition(0, m_a.m_Rt.position);
            m_lr.SetPosition(1, m_b.m_Rt.position);
        }
    }

    /// <summary>
    /// Dùng khi đang drag để cập nhật điểm B theo chuột.
    /// </summary>
    public void UpdateB(Vector3 b)
    {
        if (m_lr != null && m_lr.positionCount >= 2)
            m_lr.SetPosition(1, b);
    }
}
