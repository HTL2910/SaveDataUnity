// Assets/Scripts/Game/LevelData.cs
using UnityEngine;

[CreateAssetMenu(menuName="Graph/Level")]
public class LevelData : ScriptableObject
{
    public string m_nameKey;
    public float m_showSeconds = 5f;
    [TextArea] public string m_binary; // N:bits
}
