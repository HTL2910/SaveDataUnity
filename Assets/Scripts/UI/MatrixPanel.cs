// Assets/Scripts/UI/MatrixPanel.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixPanel : MonoBehaviour
{
    public RectTransform gridParent;
    public Toggle cellTogglePrefab;
    public TMP_Text cellLabelPrefab;

    private GraphData data;
    private bool editable;

    public void Build(GraphData g, bool editableCells)
    {
        Clear();
        data = g; editable = editableCells;
        int n = g.N;
        var grid = gridParent.GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = n;

        for (int i=0;i<n;i++)
            for (int j=0;j<n;j++)
            {
                if (editable && i!=j)
                {
                    var t = Instantiate(cellTogglePrefab, gridParent);
                    bool v = g.Get(i,j);
                    t.isOn = v;
                    int ii=i, jj=j;
                    t.onValueChanged.AddListener(val=>{
                        data.SetUndirected(ii, jj, val);
                    });
                }
                else
                {
                    var lab = Instantiate(cellLabelPrefab, gridParent);
                    lab.text = (i==j?0:(g.Get(i,j)?1:0)).ToString();
                }
            }
    }

    public void Clear()
    {
        for (int i = gridParent.childCount-1; i>=0; i--)
            Destroy(gridParent.GetChild(i).gameObject);
    }

    public void SetActive(bool on) => gameObject.SetActive(on);
}
