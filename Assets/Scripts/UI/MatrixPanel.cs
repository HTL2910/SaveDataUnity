// Assets/Scripts/UI/MatrixPanel.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixPanel : MonoBehaviour
{
    public RectTransform m_graphCenter; // trung tâm của các node trong đồ thị
    public RectTransform m_nodePrefab;  // prefab của node để instantiate xung quanh center
    public int m_count = 4;            // tổng số lượng node
    public float m_radius = 100f;         // bán kính

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        float angleStep = 360f / m_count;
        for (int i = 0; i < m_count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(100.0f, 100.0f);

            RectTransform node = Instantiate(m_nodePrefab, m_graphCenter);
            node.anchoredPosition = offset; // đối xứng quanh center
            node.name = $"Node_{i}";
        }
    }
}
