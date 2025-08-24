using UnityEngine;

public class MatrixUIController : MonoBehaviour
{
    [SerializeField] private GameObject m_gridPrefab;
    public int m_height=5;
    public int m_width=7;
    [SerializeField] Grid[,] m_grids;
    private void Start()
    {
        m_grids = new Grid[m_height, m_width];
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                GameObject grid = Instantiate(m_gridPrefab, transform);
                grid.gameObject.name = $"Grid_{i}_{j}";
                m_grids[i, j] = grid.GetComponent<Grid>();
            }
        }
    }
}
