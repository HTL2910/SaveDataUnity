using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemManager : MonoBehaviour
{
    protected GameManager gameManager;
    public GameObject colorButton;
    public GameObject adjacenButton;
    public GameObject columnButton;
    public GameObject rowButton;
    private void Start()
    {
        gameManager = GameManager.instance;
        if (gameManager != null)
        {
            GetDataAll();
            Debug.Log("GetData");
        }
        else
        {
            Debug.LogError("Not GameManager");
        }
    }

    public void GetDataAll()
    {
        GetData(colorButton);
        GetData(adjacenButton);
        GetData(columnButton);
        GetData(rowButton);
    }

    public void GetData(GameObject gameObject)
    {
        if (gameObject.GetComponent<CountBombButton>())
        {
            CountBombButton count = gameObject.GetComponent<CountBombButton>();
            string type = count.TypeBomb;

            switch (type)
            {
                case "Color":
                    count.countBomb = gameManager.gameData.countColorBomb;
                    break;
                case "Adjacen":
                    count.countBomb = gameManager.gameData.countAdjacenBomb;
                    break;
                case "Column":
                    count.countBomb = gameManager.gameData.countColumnBomb;
                    break;
                case "Row":
                    count.countBomb = gameManager.gameData.countRowBomb;
                    break;
                default:
                    Debug.Log("wrong type");
                    break;

            }
        }
        else
        {
            Debug.Log("Not have");
        }
    }
   
}
