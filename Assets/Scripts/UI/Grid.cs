using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private string m_number;
    public string Number
    {
        get { return m_number; }
        set { SetNumber(value); }
    }
    [SerializeField] private TextMeshProUGUI m_numberText;

    public void SetNumber(string number) => m_numberText.text = number;
}
