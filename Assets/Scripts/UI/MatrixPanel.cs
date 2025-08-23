// Assets/Scripts/UI/MatrixPanel.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixPanel : MonoBehaviour
{
    public RectTransform m_gridParent;
    public Toggle m_cellTogglePrefab;
    public TMP_Text m_cellLabelPrefab;

    private GraphData m_data;
    private bool m_editable;

    public void Build(GraphData g, bool editableCells)
    {
        Clear();
        m_data = g; m_editable = editableCells;
        int n = g.m_N;
        var grid = m_gridParent.GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = n;

        for (int i=0;i<n;i++)
            for (int j=0;j<n;j++)
            {
                if (m_editable && i!=j)
                {
                    var t = Instantiate(m_cellTogglePrefab, m_gridParent);
                    bool v = g.Get(i,j);
                    t.isOn = v;
                    int ii=i, jj=j;
                    t.onValueChanged.AddListener(val=>{
                        m_data.SetUndirected(ii, jj, val);
                    });
                }
                else
                {
                    var lab = Instantiate(m_cellLabelPrefab, m_gridParent);
                    lab.text = (i==j?0:(g.Get(i,j)?1:0)).ToString();
                }
            }
    }

    public void Clear()
    {
        for (int i = m_gridParent.childCount-1; i>=0; i--)
            Destroy(m_gridParent.GetChild(i).gameObject);
    }

    public void SetActive(bool on) => gameObject.SetActive(on);
}
