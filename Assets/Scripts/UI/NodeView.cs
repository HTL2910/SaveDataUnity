using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeView : MonoBehaviour
{
    public int Index;
    public RectTransform Rt;
    public Button Btn;
    public TMP_Text Label;

    public void Init(int index)
    {
        Index = index;
        Label.text = index.ToString();
    }
}
