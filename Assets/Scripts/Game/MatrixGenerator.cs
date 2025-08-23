// Assets/Scripts/Game/MatrixGenerator.cs
using UnityEngine;

/// <summary>
/// Sinh ma trận kề và GraphData từ số node.
/// </summary>
public class MatrixGenerator : MonoBehaviour
{
    public GraphData GenerateMatrix(int nodeCount, float edgeProbability = 0.3f, bool ensureConnected = true)
    {
        GraphData graph = new GraphData(nodeCount);

        // Thêm nodes
        for (int i = 0; i < nodeCount; i++)
            graph.AddNode(i);

        // Sinh cạnh ngẫu nhiên
        for (int i = 0; i < nodeCount; i++)
        {
            for (int j = i + 1; j < nodeCount; j++)
            {
                if (Random.value < edgeProbability)
                {
                    graph.AddEdge(i, j);
                }
            }
        }

        // Nếu yêu cầu connected => thêm chuỗi liên tiếp
        if (ensureConnected)
        {
            for (int i = 0; i < nodeCount - 1; i++)
            {
                if (!graph.IsConnected(i, i + 1))
                    graph.AddEdge(i, i + 1);
            }
        }

        Debug.Log($"Graph generated: {graph.nodes.Count} nodes, {graph.edges.Count} edges");
        return graph;
    }
}
