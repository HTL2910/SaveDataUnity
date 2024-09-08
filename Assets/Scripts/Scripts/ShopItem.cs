using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string typeBomb;
    public int countBomb;
    public int prices;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI pricesText;

    public GameObject buyButton;

    private void Start()
    {
        pricesText.text = "Prices: " + prices.ToString();
    }
    private void Update()
    {
        countText.text = "Count: " + countBomb.ToString();
    }
    public void BuyItem()
    {
        countBomb++;
    }
    public void BuyError()
    {
        gameObject.GetComponent<Image>().color = Color.red;
        StartCoroutine(ReturnWhite());

    }
    IEnumerator ReturnWhite()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Image>().color = Color.white;

    }
}
