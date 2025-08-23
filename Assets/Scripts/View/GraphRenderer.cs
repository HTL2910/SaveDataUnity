// Assets/Scripts/View/GraphRenderer.cs
using System.Collections.Generic;
using UnityEngine;

public class GraphRenderer : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform nodesParent;    // 1 Canvas child dùng Grid/absolute
    public Camera cam;
    public NodeView nodePrefab;
    public EdgeView edgePrefab;

    [Header("Layout")]
    public float radius = 300f;               // nếu Canvas pixel
    public Vector2 center = Vector2.zero;

    public List<NodeView> nodes = new();
    private Dictionary<(int,int), EdgeView> edges = new();

    public void BuildNodes(int n)
    {
        ClearAll();
        float step = 360f / n;
        for (int i = 0; i < n; i++)
        {
            var nv = Instantiate(nodePrefab, nodesParent);
            nv.Rt = nv.GetComponent<RectTransform>();
            nv.Init(i);
            float angle = Mathf.Deg2Rad * (i * step);
            nv.Rt.anchoredPosition = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            nodes.Add(nv);
        }
    }

    public void SetEdgeUndirected(int i, int j, bool on)
    {
        var key = i<j ? (i,j) : (j,i);
        if (on)
        {
            if (!edges.ContainsKey(key))
            {
                var e = Instantiate(edgePrefab, nodesParent);
                Vector3 a = nodes[key.Item1].Rt.position;
                Vector3 b = nodes[key.Item2].Rt.position;
                e.Init(a, b);
                edges[key] = e;
            }
        }
        else
        {
            if (edges.TryGetValue(key, out var e))
            {
                Destroy(e.gameObject);
                edges.Remove(key);
            }
        }
    }

    public void SyncFromData(GraphData g)
    {
        if (nodes.Count != g.N) BuildNodes(g.N);
        for (int i=0;i<g.N;i++)
            for (int j=i+1;j<g.N;j++)
                SetEdgeUndirected(i, j, g.Get(i,j));
    }

    public void ClearAll()
    {
        foreach (var n in nodes) Destroy(n.gameObject);
        nodes.Clear();
        foreach (var kv in edges) Destroy(kv.Value.gameObject);
        edges.Clear();
    }

    public NodeView GetNode(int i) => nodes[i];
}
