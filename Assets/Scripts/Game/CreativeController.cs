// Assets/Scripts/Game/CreativeController.cs
using UnityEngine;
using TMPro;

public class CreativeController : MonoBehaviour
{
    public GraphRenderer m_renderer;
    public MatrixPanel m_matrixPanel;
    public EdgeDragController m_dragger;
    public TMP_InputField m_shareField;

    private GraphData m_authored;
    private GraphData m_working;

    public void NewGraph(int n)
    {
        m_authored = new GraphData(n);
        m_renderer.BuildNodes(n);
        m_matrixPanel.Build(m_authored, editableCells:true);
        m_matrixPanel.SetActive(true);
        m_renderer.SyncFromData(m_authored);
    }

    public void PlayYourGraph()
    {
        // copy để người chơi vẽ lại
        m_working = new GraphData(m_authored.m_N);
        m_matrixPanel.SetActive(false);
        m_renderer.SyncFromData(new GraphData(m_authored.m_N));
        m_dragger.Bind(m_working);
    }

    public void ExportShareCode()
    {
        m_shareField.text = m_authored.ToBinaryString(); // ví dụ: "5:010010..."
    }

    public void ImportShareCode()
    {
        m_authored = GraphData.FromBinaryString(m_shareField.text);
        m_renderer.BuildNodes(m_authored.m_N);
        m_matrixPanel.Build(m_authored, editableCells:true);
        m_matrixPanel.SetActive(true);
        m_renderer.SyncFromData(m_authored);
    }
}
