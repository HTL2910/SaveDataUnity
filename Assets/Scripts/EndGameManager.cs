using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameType
{
    Moves,
    Time
}
[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
}
public class EndGameManager : MonoBehaviour
{
    public EndGameRequirements endGameRequirements;
    public GameObject timeLabel;
    public GameObject movesLabel;
    public GameObject tryAgainPanel;
    public GameObject winGamePanel;
    public TextMeshProUGUI counter;
    public int currentCounterValue;
    private float timeSeconds;
    private Board board;
    private void Start()
    {
        board=FindObjectOfType<Board>();
        SetGameType();
        SetUpGame();
    }
    void SetGameType()
    {
        if (board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    endGameRequirements = board.world.levels[board.level].endGameRequirements;
                }
            }
        }
    }
    void SetUpGame()
    {
        currentCounterValue=endGameRequirements.counterValue;
        if(endGameRequirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timeSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }
    public void DecreaseCounterValue()
    {
        if (board.currentStates != GameStates.Pause)
        {

            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            if (currentCounterValue <= 0)
            {
                LoseGame();
               
            }
        }
    }
    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentStates = GameStates.Lose;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelAnimController fadePanel = FindObjectOfType<FadePanelAnimController>();
        fadePanel.GameOver();
    }
    public void WinGame()
    {
        winGamePanel.SetActive(true);
        board.currentStates= GameStates.Win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        
    }
    private void Update()
    {
        if (endGameRequirements.gameType == GameType.Time &&currentCounterValue>0)
        {
            timeSeconds -= Time.deltaTime;
            if(timeSeconds<=0)
            {
                DecreaseCounterValue();
                timeSeconds = 1;
            }
        }
    }
}
