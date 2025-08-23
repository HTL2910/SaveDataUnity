using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public TMP_Text correctText;
    public TMP_Text totalText;
    public TMP_Text edgeCountText;
    public TMP_Text explainText;

    public void Show(int correct, int total, int edgeCount, string explain)
    {
        gameObject.SetActive(true);
        correctText.text = correct.ToString();
        totalText.text = total.ToString();
        edgeCountText.text = edgeCount.ToString();
        explainText.text = explain;
    }
}
