using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public List<Gun> listGun;

    public Image gunImage;
    public TextMeshProUGUI nameGunText;
    public GameObject attributeGunPrefab;
    public Transform attributeParent;
    public GameObject gunPrefabs;
    public Transform gunScrollViewContent;
    int selectIndex = 0;
    private void Start()
    {
        CreateUI();
        UpdateGunIndex();
    }

    private void CreateUI()
    {
       

        for (int i = 0; i < listGun.Count; i++)
        {
            GameObject gun = Instantiate(gunPrefabs);
            gun.transform.SetParent(gunScrollViewContent);
            gun.transform.position = gunScrollViewContent.transform.position;
            gun.transform.localScale = Vector3.one;
            gun.transform.GetChild(0).GetComponent<Image>().sprite = listGun[i].sprite;
            if (listGun[i].status==Status.rendted)
            {
                gun.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rended out";
                gun.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.64f, 0.0f);
            }
            else if(listGun[i].status == Status.used)
            {
                gun.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Used";
                gun.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;

            }
            else
            {
                gun.transform.GetChild(1).gameObject.SetActive(false);

            }
        }
    }

    private void UpdateGunIndex()
    {
        gunImage.sprite = listGun[selectIndex].sprite;
        nameGunText.text = listGun[selectIndex].Name;
    }
}
