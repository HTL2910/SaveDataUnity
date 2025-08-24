using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private string m_number;
    public int m_numberValue;
    private bool m_isZero=true;
    public string Number
    {
        get { return m_number; }
        set { SetNumber(value); }
    }
    [SerializeField] private TextMeshProUGUI m_numberText;

    public void SetNumber(string number) => m_numberText.text = number;

    public void OnButtonClick()
    {
        Debug.Log("OnButtonClick");
        m_isZero = !m_isZero;
        m_numberText.color = m_isZero ? Color.black : Color.green;
        m_numberValue = m_isZero ? 0 : 1;
        m_numberText.text = m_numberValue.ToString();
    }
}
