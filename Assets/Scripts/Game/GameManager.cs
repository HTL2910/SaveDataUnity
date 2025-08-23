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
    public GraphRenderer renderer;
    public EdgeDragController dragger;
    public MatrixPanel matrixPanel;
    public TMP_Text timerText;
    public ResultPanel resultPanel; // 1 panel đơn giản show đúng/sai + thống kê

    [Header("Config")]
    public float matrixShowSeconds = 5f;

    private GraphData target;   // đáp án
    private GraphData working;  // người chơi

    public void StartMemoryLevel(GraphData level)
    {
        target = level;
        working = new GraphData(level.N);
        renderer.BuildNodes(level.N);

        // Hiện đúng ma trận trong 5s
        matrixPanel.Build(target, editableCells:false);
        matrixPanel.SetActive(true);
        renderer.SyncFromData(target); // option: chỉ show matrix, chưa cho drag

        StartCoroutine(MemoryFlow());
    }

    private IEnumerator MemoryFlow()
    {
        float t = matrixShowSeconds;
        while (t > 0f)
        {
            timerText.text = t.ToString("0");
            t -= Time.deltaTime;
            yield return null;
        }
        // Ẩn matrix, reset cạnh, bật drag
        matrixPanel.SetActive(false);
        renderer.SyncFromData(new GraphData(target.N));
        dragger.Bind(working); // player bắt đầu vẽ

        // Đợi người chơi bấm nút "Submit" (tạo UI gọi Validate())
    }

    public void Validate()
    {
        // So sánh working với target
        int n = target.N;
        int correct = 0, total = 0, wrong = 0;
        for (int i=0;i<n;i++)
            for (int j=i+1;j<n;j++)
            {
                total++;
                bool a = target.Get(i,j);
                bool b = working.Get(i,j);
                if (a==b) correct++;
                else wrong++;
            }

        resultPanel.Show(
            correct, total,
            target.EdgeCount(),
            ExplainMath(target)
        );
    }

    string ExplainMath(GraphData g)
    {
        // bonus kiến thức
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Bậc từng node: " +
            string.Join(", ", Enumerable.Range(0,g.N)
                          .Select(i=>$"v{i}:{g.Degree(i)}")));
        sb.AppendLine($"Số cạnh: {g.EdgeCount()}");
        sb.AppendLine($"Số tam giác (cycle 3): {g.TriangleCount()}");
        return sb.ToString();
    }
}
