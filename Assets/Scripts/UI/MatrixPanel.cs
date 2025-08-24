// Assets/Scripts/UI/MatrixPanel.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixPanel : MonoBehaviour
{
    public RectTransform m_graphCenter; // trung tâm của các node trong đồ thị
    public RectTransform m_nodePrefab;  // prefab của node để instantiate xung quanh center
    public int m_count = 4;            // tổng số lượng node
    public float m_distance = 100f;         // bán kính

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        if (m_count == 4)
        {
            // For 4 nodes, place them at the four cardinal directions
            Vector2[] offsets = new Vector2[]
            {
                new Vector2(0, m_distance),   // Top
                new Vector2(0, -m_distance),  // Bottom
                new Vector2(m_distance, 0),   // Right
                new Vector2(-m_distance, 0)   // Left
            };

            for (int i = 0; i < m_count; i++)
            {
                RectTransform node = Instantiate(m_nodePrefab, m_graphCenter);
                node.anchoredPosition = offsets[i];
                node.name = $"Node_{i}";
            }
        }
        else if (m_count == 6)
        {
            // For 6 nodes, place them in a hexagonal pattern
            float angleStep = 360f / m_count;
            for (int i = 0; i < m_count; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 offset = new Vector2(
                    m_distance * Mathf.Cos(angle),
                    m_distance * Mathf.Sin(angle)
                );

                RectTransform node = Instantiate(m_nodePrefab, m_graphCenter);
                node.anchoredPosition = offset;
                node.name = $"Node_{i}";
            }
        }
        else
        {
            Debug.LogWarning("Unsupported number of nodes. Only 4 or 6 nodes are supported.");
        }
    }
}
