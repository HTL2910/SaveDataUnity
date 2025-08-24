    // Assets/Scripts/Game/GameManager.cs
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;

public enum GameMode { Memory, Creative }

public class GameManager : MonoBehaviour
{
    public GraphRenderer m_graphRenderer;
    
    
    /// <summary>
    /// Old code
    /// </summary>
    [Header("Refs")]
    public GraphRenderer m_renderer;         // vẽ node + cạnh
    public EdgeDragController m_dragger;     // xử lý kéo cạnh
    public MatrixPanel m_matrixPanel;        // panel hiện ma trận
    public TMP_Text m_timerText;             // UI đếm ngược
    public ResultPanel m_resultPanel;        // panel kết quả

    public static GameManager Instance { get; private set; }

    [Header("Config")]
    public float m_matrixShowSeconds = 5f;

    private GraphData m_target;   // đáp án
    private GraphData m_working;  // người chơi

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Khởi động level chế độ Memory.
    /// </summary>
    public void StartMemoryLevel(GraphData level)
    {
        m_target = level;
        m_working = new GraphData(level.m_N);

        // Vẽ node sẵn
        m_renderer.BuildNodes(level.m_N);

        // Hiện đúng ma trận trong vài giây
        m_matrixPanel.Generate();

        // Render đáp án để player ghi nhớ
        m_renderer.SyncFromData(m_target);

        StartCoroutine(MemoryFlow());
    }

    private IEnumerator MemoryFlow()
    {
        float t = m_matrixShowSeconds;
        while (t > 0f)
        {
            if (m_timerText) m_timerText.text = t.ToString("0");
            t -= Time.deltaTime;
            yield return null;
        }

        // Ẩn matrix, reset cạnh, bật drag để player vẽ lại
        m_matrixPanel.Generate();
        m_renderer.SyncFromData(new GraphData(m_target.m_N));
        m_dragger.Bind(m_working);
    }

    /// <summary>
    /// Nút "Submit" gọi hàm này để chấm điểm.
    /// </summary>
    public void Validate()
    {
        int n = m_target.m_N;
        int correct = 0, total = 0, wrong = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                total++;
                bool a = m_target.Get(i, j);
                bool b = m_working.Get(i, j);
                if (a == b) correct++;
                else wrong++;
            }
        }

        m_resultPanel.Show(
            correct, total,
            m_target.EdgeCount(),
            ExplainMath(m_target)
        );
    }

    private string ExplainMath(GraphData g)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Bậc từng node: " +
            string.Join(", ",
                Enumerable.Range(0, g.m_N)
                          .Select(i => $"v{i}:{g.Degree(i)}")));

        sb.AppendLine($"Số cạnh: {g.EdgeCount()}");
        sb.AppendLine($"Số tam giác (cycle 3): {g.TriangleCount()}");
        return sb.ToString();
    }
    public void OnNodeSelected(NodeView node)
    {
        // TODO: xử lý khi chọn node (tuỳ mode Memory/Creative)
        Debug.Log($"Node {node.m_Index} selected");

        // Ví dụ Creative Mode: bật drag để nối cạnh
        if (m_dragger != null)
        {
            m_dragger.StartDrag(node);
        }
    }
}
