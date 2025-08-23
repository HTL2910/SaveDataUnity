using UnityEngine;

/// <summary>
/// Điều khiển cạnh giữa 2 node.
/// </summary>
[RequireComponent(typeof(EdgeView))]
public class EdgeController : MonoBehaviour
{
    private EdgeView view;
    private NodeView nodeA;
    private NodeView nodeB;

    private void Awake()
    {
        view = GetComponent<EdgeView>();
    }

    /// <summary>
    /// Khởi tạo cạnh nối giữa 2 NodeView.
    /// </summary>
    public void Init(NodeView a, NodeView b)
    {
        nodeA = a;
        nodeB = b;
        view.SetNodes(nodeA, nodeB);
    }

    public void OnEdgeClicked()
    {
        if (nodeA != null && nodeB != null)
        {
            Debug.Log($"Edge clicked: {nodeA.m_Index} - {nodeB.m_Index}");
            // TODO: toggle highlight hoặc delete cạnh
        }
    }
}
