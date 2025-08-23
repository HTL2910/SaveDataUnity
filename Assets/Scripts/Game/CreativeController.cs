// Assets/Scripts/Game/CreativeController.cs
using UnityEngine;
using TMPro;

public class CreativeController : MonoBehaviour
{
    public GraphRenderer renderer;
    public MatrixPanel matrixPanel;
    public EdgeDragController dragger;
    public TMP_InputField shareField;

    private GraphData authored;
    private GraphData working;

    public void NewGraph(int n)
    {
        authored = new GraphData(n);
        renderer.BuildNodes(n);
        matrixPanel.Build(authored, editableCells:true);
        matrixPanel.SetActive(true);
        renderer.SyncFromData(authored);
    }

    public void PlayYourGraph()
    {
        // copy để người chơi vẽ lại
        working = new GraphData(authored.N);
        matrixPanel.SetActive(false);
        renderer.SyncFromData(new GraphData(authored.N));
        dragger.Bind(working);
    }

    public void ExportShareCode()
    {
        shareField.text = authored.ToBinaryString(); // ví dụ: "5:010010..."
    }

    public void ImportShareCode()
    {
        authored = GraphData.FromBinaryString(shareField.text);
        renderer.BuildNodes(authored.N);
        matrixPanel.Build(authored, editableCells:true);
        matrixPanel.SetActive(true);
        renderer.SyncFromData(authored);
    }
}
