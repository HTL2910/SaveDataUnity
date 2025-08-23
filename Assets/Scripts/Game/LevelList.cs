using UnityEngine;

[CreateAssetMenu(menuName = "Graph/LevelList")]
public class LevelList : ScriptableObject
{
    public LevelData[] levels;
    public void StartLevelIndex(int idx)
    {
        var ld = levels[idx];
        //matrixShowSeconds = ld.showSeconds;
        //StartMemoryLevel(GraphData.FromBinaryString(ld.binary));
    }

}
