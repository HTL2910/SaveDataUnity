using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Header("Scene")]
    public string leveltoLoad;
    [Header("index")]
    public int level;
    public int countLevel=100;
    int page = 24;
    int pageIndex=1;
    public int unclockLevel;
    [Header("Object")]
    public GameObject confirmPanel;
    public GameObject prefabLevelButton;
    public GameObject preViousButton;
    public GameObject nextButton;

    public TextMeshProUGUI pageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI starTextConfirm;
    public TextMeshProUGUI scoreTextConfirm;
    int star;

    private void Start()
    {
        unclockLevel = PlayerPrefs.GetInt("Unclock Level", 1);
        CreateButtonLevel();
        Page();
    }
    
    public void Play()
    {
       
        PlayerPrefs.SetInt("Current Level", level - 1);
        PlayerPrefs.Save();
        //Debug.Log("Level" + level.ToString());
        SceneManager.LoadScene(leveltoLoad);
    }
    public void CreateButtonLevel()
    {
        for(int i = 0; i < countLevel; i++)
        {
            GameObject levelButton=Instantiate(prefabLevelButton,transform.position,Quaternion.identity);
            if(i<unclockLevel)
            {
                levelButton.GetComponent<LevelButton>().isActive = true;
            }
            else
            {
                levelButton.GetComponent<LevelButton>().isActive = false;
            }
            levelButton.GetComponent<LevelButton>().ActivateStars(levelButton.GetComponent<LevelButton>().stars.Length, false);
        
            levelButton.transform.SetParent(transform,false);
            if (i < unclockLevel-1)
            {
                for (int j = 1; j < unclockLevel; j++)
                {
                    int countStar = PlayerPrefs.GetInt("Star in Level_" + j, 0);
                    levelButton.GetComponent<LevelButton>().ActivateStars(countStar, true);

                }
            }
            
            int index = i;
            Button btn=levelButton.transform.GetChild(0).GetComponent<Button>();
            levelButton.transform.GetChild(0).transform.GetChild(0).
                GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            btn.onClick.AddListener(() => ConfirmPanel(index));
        }
    }
    public void ConfirmPanel(int level)
    {
        levelText.text = "LEVEL: "+(level+1).ToString();
        this.level = level+1;
        confirmPanel.SetActive(true);
        starTextConfirm.text = "" + PlayerPrefs.GetInt("Star in Level_" + (level + 1).ToString(), 0)+" / "+"3";
        scoreTextConfirm.text= "" + PlayerPrefs.GetInt("Score in Level_" +(level+1).ToString(), 0);
    }

    void PageLevel()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if(i>=page*(pageIndex-1) && i < page * pageIndex)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void Page()
    {
        PageLevel();
        ShowPageButton();
        ShowPageText();
    }
    private void ShowPageText()
    {
        pageText.text = "Page: " + (pageIndex).ToString() + " / " + (Mathf.CeilToInt((float)countLevel / page).ToString());
    }
    public void NextPage()
    {
        pageIndex++;
        Page();
    }
    public void PreviousPage()
    {
        pageIndex--;
        Page();
    }
    private void ShowPageButton()
    {
        if (pageIndex <= 1)
        {
            preViousButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if(pageIndex >= Mathf.CeilToInt((float)countLevel / page))
        {
            preViousButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            preViousButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
