// Assets/Scripts/Game/LevelData.cs
using UnityEngine;

[CreateAssetMenu(menuName="Graph/Level")]
public class LevelData : ScriptableObject
{
    public string nameKey;
    public float showSeconds = 5f;
    [TextArea] public string binary; // N:bits
}
