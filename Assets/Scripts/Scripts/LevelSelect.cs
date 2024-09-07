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
    public int countLevel=500;
    int page = 15;


    [Header("Object")]
    public GameObject confirmPanel;
    public GameObject prefabLevelButton;
    public GameObject preViousButton;
    public GameObject nextButton;

    public TextMeshProUGUI pageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI starTextConfirm;
    public TextMeshProUGUI scoreTextConfirm;
    public GameManager gameManager;
    private void Awake()
    {
       
        
    }
    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.gameData =gameManager.LoadGameData(countLevel);
        gameManager.gameData.totalLevel=countLevel;
        gameManager.SaveGameData();
      
        CreateButtonLevel();
    
    }

  

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level",level-1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(leveltoLoad);
    }
  
    public void CreateButtonLevel()
    {
  

        for (int i = 0; i < gameManager.gameData.totalLevel; i++)
        {
            GameObject levelButton = Instantiate(prefabLevelButton, transform.position, Quaternion.identity);
            LevelButton levelBtn = levelButton.GetComponent<LevelButton>();
            if (i < gameManager.gameData.unclockLevel)
            {
                levelBtn.isActive = true;
            }
            else
            {
                levelBtn.isActive = false;
            }
            levelBtn.ActivateStars(levelBtn.stars.Length, false);
            int countStar = gameManager.gameData.levels[i].stars;
            levelBtn.ActivateStars(countStar, true);
            levelButton.transform.SetParent(transform, false);

            int index = i;
            Button btn = levelButton.transform.GetChild(0).GetComponent<Button>();
            levelButton.transform.GetChild(0).transform.GetChild(0).
                GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            btn.onClick.AddListener(() => ConfirmPanel(index));
        }



    }
    public void ConfirmPanel(int level)
    {
        
        levelText.text = "LEVEL: "+(level+1).ToString();
        this.level = level+1;
        confirmPanel.SetActive(true);
        if (gameManager.gameData.levels[level] != null)
        {
            starTextConfirm.text = "" + gameManager.gameData.levels[level].stars.ToString() + " / " + "3";
            scoreTextConfirm.text = "" + gameManager.gameData.levels[level].score.ToString();
        }
        
    }
    private void Update()
    {
        Page();
    }
    void PageLevel()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if(i>=page* (gameManager.gameData.pageIndex - 1) && i < page * gameManager.gameData.pageIndex)
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
        pageText.text = "Page: " + (gameManager.gameData.pageIndex).ToString() + " / " + (Mathf.CeilToInt((float)countLevel / page).ToString());
    }
    public void NextPage()
    {
        gameManager.gameData.pageIndex++;
        if (gameManager.gameData.unclockLevel > (page*(gameManager.gameData.pageIndex-1)))
        {
            gameManager.SaveGameData();
        }
    }
    public void PreviousPage()
    {
        gameManager.gameData.pageIndex--;
        if (gameManager.gameData.unclockLevel > (page * (gameManager.gameData.pageIndex-1)))
        {
            gameManager.SaveGameData();
        }
    }
    private void ShowPageButton()
    {
        if (gameManager.gameData.pageIndex <= 1)
        {
            preViousButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if (gameManager.gameData.pageIndex >= Mathf.CeilToInt((float)countLevel / page))
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
