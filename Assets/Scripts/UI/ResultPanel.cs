using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public TMP_Text m_correctText;
    public TMP_Text m_totalText;
    public TMP_Text m_edgeCountText;
    public TMP_Text m_explainText;

    public void Show(int correct, int total, int edgeCount, string explain)
    {
        gameObject.SetActive(true);
        m_correctText.text = correct.ToString();
        m_totalText.text = total.ToString();
        m_edgeCountText.text = edgeCount.ToString();
        m_explainText.text = explain;
    }
}
