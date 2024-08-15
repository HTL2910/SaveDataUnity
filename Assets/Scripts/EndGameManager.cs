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
    public TextMeshProUGUI counter;
    public int currentCounterValue;
    private float timeSeconds;
    private void Start()
    {
        SetUpGame();
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
        currentCounterValue --;
        counter.text = "" + currentCounterValue;
        if (currentCounterValue <=0)
        {
            currentCounterValue=0;
            counter.text = "" + currentCounterValue;
        
            Debug.Log("You Lose");
        }
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
