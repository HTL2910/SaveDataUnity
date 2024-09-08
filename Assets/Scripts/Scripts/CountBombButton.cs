using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountBombButton : MonoBehaviour
{
    public string TypeBomb;
    public GameObject noneBomb;
    public GameObject haveBomb;
    public int countBomb;
    private void Update()
    {
        CountBomb();
    }
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
    public void Error()
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
