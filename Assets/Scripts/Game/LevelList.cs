using UnityEngine;

[CreateAssetMenu(menuName = "Graph/LevelList")]
public class LevelList : ScriptableObject
{
    public LevelData[] m_levels;
    public void StartLevelIndex(int idx)
    {
        var ld = m_levels[idx];
        //matrixShowSeconds = ld.showSeconds;
        //StartMemoryLevel(GraphData.FromBinaryString(ld.binary));
    }

}
