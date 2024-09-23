using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemManager : MonoBehaviour
{
    public GameObject colorButton;
    public GameObject adjacenButton;
    public GameObject columnButton;
    public GameObject rowButton;
    private void Start()
    {
        GetDataAll();
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
                    count.countBomb = PlayerPrefs.GetInt("countColorBomb", 0);
                    break;
                case "Adjacen":
                    count.countBomb = PlayerPrefs.GetInt("countAdjacenBomb", 0);
                    break;
                case "Column":
                    count.countBomb = PlayerPrefs.GetInt("countColumnBomb", 0);
                    break;
                case "Row":
                    count.countBomb = PlayerPrefs.GetInt("countRowBomb", 0);
                    break;
                default:
                    Debug.Log("wrong type");
                    break;

            }
        }
        else
        {
            Debug.Log("Not have CountBombButton");
        }
    }
   
}
