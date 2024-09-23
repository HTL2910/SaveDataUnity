using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject bombColor;
    public GameObject bombAdjacen;
    public GameObject bombColumn;
    public GameObject bombRow;

    public TextMeshProUGUI totalScoreText;

    protected int totalScore;

    private void Start()
    {
        LoadCountBomb(bombColor);
        LoadCountBomb(bombAdjacen);
        LoadCountBomb(bombColumn);
        LoadCountBomb(bombRow);
        totalScore = PlayerPrefs.GetInt("Total_Score", 0);
    }
    private void Update()
    {
        totalScoreText.text = "Total: "+totalScore.ToString();
    }
    public void LoadCountBomb(GameObject gameObject)
    {
        if(gameObject.GetComponent<ShopItem>() != null)
        {
            ShopItem item=gameObject.GetComponent<ShopItem>();
            string type= item.typeBomb;
            switch(type)
            {
                case "Color":
                    item.countBomb = PlayerPrefs.GetInt("countColorBomb", 0);
                    break;
                case "Adjacen":
                    item.countBomb = PlayerPrefs.GetInt("countAdjacenBomb", 0);
                    break;
                case "Column":
                    item.countBomb = PlayerPrefs.GetInt("countColumnBomb", 0);
                    break;
                case "Row":
                    item.countBomb = PlayerPrefs.GetInt("countRowBomb", 0);
                    break;
                default:
                    Debug.Log("wrong type");
                    break;
            }
            item.buyButton.GetComponent<Button>().onClick.AddListener(() => SaveCountBomb(gameObject));
        }
    }
    private void SaveCountBomb(GameObject gameObject)
    {
        ShopItem item = gameObject.GetComponent<ShopItem>();
        string type = item.typeBomb;
        if (PlayerPrefs.GetInt("Total_Score", 0) > item.prices)
        {
            item.BuyItem();
            totalScore -= item.prices;
            PlayerPrefs.SetInt("Total_Score", totalScore);
            switch (type)
            {
                case "Color":
                    PlayerPrefs.SetInt("countColorBomb", item.countBomb);
                    break;
                case "Adjacen":
                    PlayerPrefs.SetInt("countAdjacenBomb", item.countBomb);
               
                    break;
                case "Column":
                    PlayerPrefs.SetInt("countColumnBomb", item.countBomb);
                    break;
                case "Row":
                    PlayerPrefs.SetInt("countRowBomb", item.countBomb);
                    break;
                default:
                    Debug.Log("wrong type");
                    break;
            }
        }
        else
        {
            item.BuyError();
        }
        PlayerPrefs.Save();
        Debug.Log("Save");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Splash");
    }
}
