using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}
public class GoalsManager : MonoBehaviour
{
    public BlankGoal[] allLevelGoals;
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();

    public GameObject goalPrefabs;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    private EndGameManager endGame;
    private Board board;
    private void Start()
    {
        board=FindObjectOfType<Board>();
        endGame=FindObjectOfType<EndGameManager>();
        GetGoals();
        SetUpIntroGoals();
    }
    void GetGoals()
    {
        if(board!=null && board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    levelGoals = board.world.levels[board.level].levelGoals;
                    for(int i=0; i<levelGoals.Length; i++)
                    {
                        levelGoals[i].numberCollected = 0;
                    }
                }
            }
        }
    }
    void SetUpIntroGoals()
    {
        for(int i=0; i<levelGoals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefabs, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform, false);
            GoalPanel panel=goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;

            GameObject gameGoal = Instantiate(goalPrefabs, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform, false);
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }
    public void UpdateGoal()
    {
        int goalsCompleted = 0;
        for(int i=0;i<levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text= "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;

            }
        }
        if (goalsCompleted >= levelGoals.Length)
        {

            if (endGame != null)
            {
                endGame.WinGame();
            }
        }
    }
    public void CompareGoal(string goalToCompare)
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if (goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
}
