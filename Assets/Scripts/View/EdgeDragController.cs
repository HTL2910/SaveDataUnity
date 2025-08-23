// Assets/Scripts/View/EdgeDragController.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeDragController : MonoBehaviour
{
    public GraphRenderer m_graph;
    public GraphData m_working; // đồ thị người chơi đang vẽ
    public EdgeView m_tempEdge;
    private int? m_from = null;

    void Start()
    {
        // gán sự kiện click cho node
        foreach (var nv in m_graph.m_nodes)
            nv.m_Btn.onClick.AddListener(()=> OnNodeClicked(nv.m_Index));
    }

    public void Bind(GraphData data)
    {
        m_working = data;
        m_graph.SyncFromData(m_working);
        // rebind click (nếu nodes mới)
        foreach (var nv in m_graph.m_nodes)
        {
            nv.m_Btn.onClick.RemoveAllListeners();
            nv.m_Btn.onClick.AddListener(()=> OnNodeClicked(nv.m_Index));
        }
    }

    // Method for GameManager compatibility
    public void StartDrag(NodeView node)
    {
        if (node != null)
        {
            OnNodeClicked(node.m_Index);
        }
    }

    void Update()
    {
        if (m_tempEdge!=null)
        {
            Vector3 mouse = Input.mousePosition;
            m_tempEdge.UpdateB(mouse);
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(m_tempEdge.gameObject);
                m_tempEdge = null;
                m_from = null;
            }
        }
    }

    void OnNodeClicked(int idx)
    {
        if (m_from==null)
        {
            m_from = idx;
            m_tempEdge = Instantiate(m_graph.m_edgePrefab, m_graph.m_nodesParent);
            m_tempEdge.Init(m_graph.GetNode(idx).m_Rt.position, Input.mousePosition);
        }
        else
        {
            int i = m_from.Value;
            int j = idx;
            if (i!=j)
            {
                bool newVal = !m_working.Get(i,j);
                m_working.SetUndirected(i, j, newVal);
                m_graph.SetEdgeUndirected(i, j, newVal);
            }
            Destroy(m_tempEdge?.gameObject);
            m_tempEdge = null;
            m_from = null;
        }
    }
}
