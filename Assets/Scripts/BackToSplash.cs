using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplash : MonoBehaviour
{
    public string sceneToLoad;
    private ScoreManager scoreManager;
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    public void OkWin()
    {
        int totalScore = PlayerPrefs.GetInt("Total_Score",0);
        totalScore += scoreManager.score;
        PlayerPrefs.SetInt("Total_Score", totalScore);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneToLoad);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
