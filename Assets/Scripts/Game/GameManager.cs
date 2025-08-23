// Assets/Scripts/Game/GameManager.cs
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public enum GameMode { Memory, Creative }

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public GraphRenderer m_renderer;
    public EdgeDragController m_dragger;
    public MatrixPanel m_matrixPanel;
    public TMP_Text m_timerText;
    public ResultPanel m_resultPanel; // 1 panel đơn giản show đúng/sai + thống kê

    [Header("Config")]
    public float m_matrixShowSeconds = 5f;

    private GraphData m_target;   // đáp án
    private GraphData m_working;  // người chơi

    public void StartMemoryLevel(GraphData level)
    {
        m_target = level;
        m_working = new GraphData(level.m_N);
        m_renderer.BuildNodes(level.m_N);

        // Hiện đúng ma trận trong 5s
        m_matrixPanel.Build(m_target, editableCells:false);
        m_matrixPanel.SetActive(true);
        m_renderer.SyncFromData(m_target); // option: chỉ show matrix, chưa cho drag

        StartCoroutine(MemoryFlow());
    }

    private IEnumerator MemoryFlow()
    {
        float t = m_matrixShowSeconds;
        while (t > 0f)
        {
            m_timerText.text = t.ToString("0");
            t -= Time.deltaTime;
            yield return null;
        }
        // Ẩn matrix, reset cạnh, bật drag
        m_matrixPanel.SetActive(false);
        m_renderer.SyncFromData(new GraphData(m_target.m_N));
        m_dragger.Bind(m_working); // player bắt đầu vẽ

        // Đợi người chơi bấm nút "Submit" (tạo UI gọi Validate())
    }

    public void Validate()
    {
        // So sánh working với target
        int n = m_target.m_N;
        int correct = 0, total = 0, wrong = 0;
        for (int i=0;i<n;i++)
            for (int j=i+1;j<n;j++)
            {
                total++;
                bool a = m_target.Get(i,j);
                bool b = m_working.Get(i,j);
                if (a==b) correct++;
                else wrong++;
            }

        m_resultPanel.Show(
            correct, total,
            m_target.EdgeCount(),
            ExplainMath(m_target)
        );
    }

    string ExplainMath(GraphData g)
    {
        // bonus kiến thức
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Bậc từng node: " +
            string.Join(", ", Enumerable.Range(0,g.m_N)
                          .Select(i=>$"v{i}:{g.Degree(i)}")));
        sb.AppendLine($"Số cạnh: {g.EdgeCount()}");
        sb.AppendLine($"Số tam giác (cycle 3): {g.TriangleCount()}");
        return sb.ToString();
    }
}
