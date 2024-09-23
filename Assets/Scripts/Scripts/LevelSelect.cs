using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Header("Scene")]
    public string leveltoLoad;
    [Header("index")]
    public int level;
    
    [Header("Object")]
    public GameObject confirmPanel;
    public GameObject prefabLevelButton;
    public GameObject preViousButton;
    public GameObject nextButton;

    public TextMeshProUGUI pageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI starTextConfirm;
    public TextMeshProUGUI scoreTextConfirm;
    public TextMeshProUGUI logText;
    int pageIndex=1;
    int page = 15;
    int totalLevel = 500;
    //int unclockLevel = 500;

    private void Start()
    {
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
        for (int i = 0; i < totalLevel; i++)
        {
            GameObject levelButton = Instantiate(prefabLevelButton, transform.position, Quaternion.identity);
            LevelButton levelBtn = levelButton.GetComponent<LevelButton>();
            //if (i < unclockLevel)
            //{
            //    levelBtn.isActive = true;
            //}
            //else
            //{
            //    levelBtn.isActive = false;
            //}
            levelBtn.isActive = true;
            levelBtn.ActivateStars(levelBtn.stars.Length, false);
            int countStar = PlayerPrefs.GetInt("Star in Level_" + (i + 1), 0);
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
        starTextConfirm.text = "" + PlayerPrefs.GetInt("Star in Level_" + (level + 1).ToString(), 0) + " / " + "3";
        scoreTextConfirm.text = "" + PlayerPrefs.GetInt("Score in Level_" + (level + 1).ToString(), 0);
    }
    private void Update()
    {
        Page();
    }
    void PageLevel()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if(i>=page* (pageIndex - 1) && i < page * pageIndex)
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
        pageText.text = "Page "+pageIndex+ " / "  + Mathf.CeilToInt((float)totalLevel / page);
    }
    public void NextPage()
    {
       pageIndex++;

    }
    public void PreviousPage()
    {
        pageIndex--;

    }
    private void ShowPageButton()
    {
        if (pageIndex <= 1)
        {
            preViousButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if (pageIndex >= Mathf.CeilToInt((float)totalLevel / page))
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
    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }
}
