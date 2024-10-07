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
    [SerializeField]int selectIndex = 0;
 

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI dispersionText;
    public TextMeshProUGUI rateOfFireText;
    public TextMeshProUGUI reloadSpeedText;
    public TextMeshProUGUI ammunitionText;
    private void Start()
    {
        CreateUI();
    }

    private void CreateUI()
    {
       

        for (int i = 0; i < listGun.Count; i++)
        {
            GameObject gun = Instantiate(gunPrefabs);
            gun.transform.SetParent(gunScrollViewContent,false);
            gun.transform.GetChild(0).GetComponent<Image>().sprite = listGun[i].sprite;
           
            int index = i;
            gun.GetComponent<Button>().onClick.AddListener(() => ChangeIndex(index));
        }
        UpdateGunIndex();
        ChangeStatus();
    }
    void ChangeStatus()
    {
        for (int i = 0; i < listGun.Count; i++)
        {
            TextMeshProUGUI textTmp = gunScrollViewContent.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (listGun[i].status == Status.rendted)
            {
                textTmp.text = "Rended out";
                textTmp.color = new Color(1.0f, 0.64f, 0.0f);
            }
            else if (listGun[i].status == Status.used)
            {
                textTmp.text = "Used";
                textTmp.color = Color.green;

            }
            else
            {
                gunScrollViewContent.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    void ChangeIndex(int index)
    {
        selectIndex = index;
        UpdateGunIndex();
    }
    private void UpdateGunIndex() 
    { 
        gunImage.sprite = listGun[selectIndex].sprite;
        nameGunText.text = listGun[selectIndex].Name;
        damageText.text = listGun[selectIndex].damage.ToString();
        dispersionText.text = listGun[selectIndex].dispersion.ToString();
        rateOfFireText.text = listGun[selectIndex].rateOfFire.ToString()+ " RPM";
        reloadSpeedText.text= listGun[selectIndex].reloadSpeed.ToString()+ '%';
        ammunitionText.text = listGun[selectIndex].ammunition.ToString()+ "/100";
    }
}
