using UnityEngine;

/// <summary>
/// Điều khiển node prefab (gắn kèm NodeView).
/// </summary>
[RequireComponent(typeof(NodeView))]
public class NodeController : MonoBehaviour
{
    public NodeView m_view { get; private set; }
    private int nodeId;

    private void Awake()
    {
        m_view = GetComponent<NodeView>();
    }

    /// <summary>
    /// Khởi tạo node với id (index).
    /// </summary>
    public void Init(int id)
    {
        nodeId = id;
        m_view.Init(id);
        m_view.m_Btn.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Khi node được click.
    /// </summary>
    private void OnClick()
    {
        Debug.Log("Node clicked: " + nodeId);

        // Gửi event về GameManager (chỉ cần pass NodeView hoặc nodeId)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnNodeSelected(m_view);
            // Nếu bạn thích truyền id thay vì view:
            // GameManager.Instance.OnNodeSelected(nodeId);
        }
    }
}
