// Assets/Scripts/View/GraphRenderer.cs
using System.Collections.Generic;
using UnityEngine;

public class GraphRenderer : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform m_nodesParent;    // 1 Canvas child dùng Grid/absolute
    public Camera m_cam;
    public NodeView m_nodePrefab;
    public EdgeView m_edgePrefab;

    [Header("Layout")]
    public float m_radius = 300f;               // nếu Canvas pixel
    public Vector2 m_center = Vector2.zero;
    public bool m_useMatrixLayout = false;      // Use MatrixPanel-style layout for 4 and 6 nodes

    public List<NodeView> m_nodes = new();
    private Dictionary<(int,int), EdgeView> m_edges = new();

    public void BuildNodes(int n)
    {
        ClearAll();
        
        if (m_useMatrixLayout && (n == 4 || n == 6))
        {
            BuildNodesMatrixLayout(n);
        }
        else
        {
            BuildNodesCircularLayout(n);
        }
    }

    private void BuildNodesMatrixLayout(int n)
    {
        if (n == 4)
        {
            // For 4 nodes, place them at the four cardinal directions (like MatrixPanel)
            Vector2[] offsets = new Vector2[]
            {
                new Vector2(0, m_radius),       // Top
                new Vector2(0, -m_radius),      // Bottom
                new Vector2(m_radius, 0),       // Right
                new Vector2(-m_radius, 0)       // Left
            };

            for (int i = 0; i < n; i++)
            {
                var nv = Instantiate(m_nodePrefab, m_nodesParent);
                nv.m_Rt = nv.GetComponent<RectTransform>();
                nv.Init(i);
                nv.m_Rt.anchoredPosition = m_center + offsets[i];
                nv.name = $"Node_{i}";
                m_nodes.Add(nv);
            }
        }
        else if (n == 6)
        {
            // For 6 nodes, place them in a hexagonal pattern (like MatrixPanel)
            float angleStep = 360f / n;
            for (int i = 0; i < n; i++)
            {
                var nv = Instantiate(m_nodePrefab, m_nodesParent);
                nv.m_Rt = nv.GetComponent<RectTransform>();
                nv.Init(i);
                
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 offset = new Vector2(
                    m_radius * Mathf.Cos(angle),
                    m_radius * Mathf.Sin(angle)
                );
                
                nv.m_Rt.anchoredPosition = m_center + offset;
                nv.name = $"Node_{i}";
                m_nodes.Add(nv);
            }
        }
    }

    private void BuildNodesCircularLayout(int n)
    {
        // Original circular layout for other numbers of nodes
        float step = 360f / n;
        for (int i = 0; i < n; i++)
        {
            var nv = Instantiate(m_nodePrefab, m_nodesParent);
            nv.m_Rt = nv.GetComponent<RectTransform>();
            nv.Init(i);
            float angle = Mathf.Deg2Rad * (i * step);
            nv.m_Rt.anchoredPosition = m_center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * m_radius;
            nv.name = $"Node_{i}";
            m_nodes.Add(nv);
        }
    }

    public void SetEdgeUndirected(int i, int j, bool on)
    {
        var key = i<j ? (i,j) : (j,i);
        if (on)
        {
            if (!m_edges.ContainsKey(key))
            {
                var e = Instantiate(m_edgePrefab, m_nodesParent);
                Vector3 a = m_nodes[key.Item1].m_Rt.position;
                Vector3 b = m_nodes[key.Item2].m_Rt.position;
                e.Init(a, b);
                m_edges[key] = e;
            }
        }
        else
        {
            if (m_edges.TryGetValue(key, out var e))
            {
                Destroy(e.gameObject);
                m_edges.Remove(key);
            }
        }
    }

    public void SyncFromData(GraphData g)
    {
        if (m_nodes.Count != g.m_N) BuildNodes(g.m_N);
        for (int i=0;i<g.m_N;i++)
            for (int j=i+1;j<g.m_N;j++)
                SetEdgeUndirected(i, j, g.Get(i,j));
    }

    public void ClearAll()
    {
        foreach (var n in m_nodes) Destroy(n.gameObject);
        m_nodes.Clear();
        foreach (var kv in m_edges) Destroy(kv.Value.gameObject);
        m_edges.Clear();
    }

    public NodeView GetNode(int i) => m_nodes[i];

    // Helper method to set layout type
    public void SetMatrixLayout(bool useMatrixLayout)
    {
        m_useMatrixLayout = useMatrixLayout;
    }

    // Helper method to set radius (distance)
    public void SetRadius(float radius)
    {
        m_radius = radius;
    }
}
