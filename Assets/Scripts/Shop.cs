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

    private GameManager manager;
    protected int totalScore;
    private void Awake()
    {
        manager=GameManager.instance;
    }
    private void Start()
    {
        if (manager != null)
        {
            manager.gameData=manager.LoadGameData();
            LoadCountBomb(bombColor);
            LoadCountBomb(bombAdjacen);
            LoadCountBomb(bombColumn);
            LoadCountBomb(bombRow);
            totalScore=manager.gameData.totalScore;
           
        }
        
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
                    item.countBomb = manager.gameData.countColorBomb;
                    break;
                case "Adjacen":
                    item.countBomb = manager.gameData.countAdjacenBomb;
                    break;
                case "Column":
                    item.countBomb = manager.gameData.countColumnBomb;
                    break;
                case "Row":
                    item.countBomb = manager.gameData.countRowBomb;
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
        if (manager.gameData.totalScore > item.prices)
        {
            item.BuyItem();
            manager.gameData.totalScore-=item.prices;
            totalScore=manager.gameData.totalScore;
            switch (type)
            {
                case "Color":
                    manager.gameData.countColorBomb = item.countBomb;
                    break;
                case "Adjacen":
                    manager.gameData.countAdjacenBomb = item.countBomb;
                    break;
                case "Column":
                    manager.gameData.countColumnBomb = item.countBomb;
                    break;
                case "Row":
                    manager.gameData.countRowBomb = item.countBomb;
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
        manager.SaveGameData();
        Debug.Log("Save");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Splash");
    }
}
