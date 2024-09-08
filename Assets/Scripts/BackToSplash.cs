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
        if(GameManager.instance != null)
        {
            GameManager gameManager= GameManager.instance;
            gameManager.gameData.totalScore += scoreManager.score;
            gameManager.SaveGameData();
        }
        SceneManager.LoadScene(sceneToLoad);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
