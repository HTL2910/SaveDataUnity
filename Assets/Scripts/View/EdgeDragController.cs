// Assets/Scripts/View/EdgeDragController.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeDragController : MonoBehaviour
{
    public GraphRenderer graph;
    public GraphData working; // đồ thị người chơi đang vẽ
    public EdgeView tempEdge;
    private int? from = null;

    void Start()
    {
        // gán sự kiện click cho node
        foreach (var nv in graph.nodes)
            nv.Btn.onClick.AddListener(()=> OnNodeClicked(nv.Index));
    }

    public void Bind(GraphData data)
    {
        working = data;
        graph.SyncFromData(working);
        // rebind click (nếu nodes mới)
        foreach (var nv in graph.nodes)
        {
            nv.Btn.onClick.RemoveAllListeners();
            nv.Btn.onClick.AddListener(()=> OnNodeClicked(nv.Index));
        }
    }

    void Update()
    {
        if (tempEdge!=null)
        {
            Vector3 mouse = Input.mousePosition;
            tempEdge.UpdateB(mouse);
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(tempEdge.gameObject);
                tempEdge = null;
                from = null;
            }
        }
    }

    void OnNodeClicked(int idx)
    {
        if (from==null)
        {
            from = idx;
            tempEdge = Instantiate(graph.edgePrefab, graph.nodesParent);
            tempEdge.Init(graph.GetNode(idx).Rt.position, Input.mousePosition);
        }
        else
        {
            int i = from.Value;
            int j = idx;
            if (i!=j)
            {
                bool newVal = !working.Get(i,j);
                working.SetUndirected(i, j, newVal);
                graph.SetEdgeUndirected(i, j, newVal);
            }
            Destroy(tempEdge?.gameObject);
            tempEdge = null;
            from = null;
        }
    }
}
