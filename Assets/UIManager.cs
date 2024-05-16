using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour ,IPointerEnterHandler
{
    // Tìm và lưu trữ tất cả các đối tượng UI Text trong game
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("UI Name: " + gameObject.name);
    }
}
