// Assets/Scripts/View/NodeView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeView : MonoBehaviour
{
    public int m_Index;
    public RectTransform m_Rt;
    public Button m_Btn;
    public TMP_Text m_Label;

    public void Init(int index)
    {
        m_Index = index;
        SetLabel($"v{index}");
    }

    public void SetLabel(string text)
    {
        if (m_Label != null)
            m_Label.text = text;
    }

    public int GetIndex() => m_Index;
}
