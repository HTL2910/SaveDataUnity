using UnityEngine;

/// <summary>
/// Quản lý graph hiện tại trong session game.
/// </summary>
public class MatrixManager : MonoBehaviour
{
    public GraphData CurrentGraph { get; private set; }

    [SerializeField] private MatrixGenerator generator;

    public void CreateNewGraph(int nodeCount)
    {
        CurrentGraph = generator.GenerateMatrix(nodeCount);
    }

    public void LoadGraph(GraphData data)
    {
        CurrentGraph = data;
    }
}
