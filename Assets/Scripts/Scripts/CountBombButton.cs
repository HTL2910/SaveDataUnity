using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountBombButton : MonoBehaviour
{
    public GameObject noneBomb;
    public GameObject haveBomb;
    public int countBomb;

    public void CountBomb()
    {
        if(countBomb <= 0)
        {
            noneBomb.SetActive(true);
            haveBomb.SetActive(false);
        }
        else
        {
            noneBomb.SetActive(false);
            haveBomb.SetActive(true);
            haveBomb.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=countBomb.ToString();
        }
    }
}
