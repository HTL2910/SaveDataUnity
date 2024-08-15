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
    public GameObject goalPrefabs;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    private void Start()
    {
        SetUpIntroGoals();
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
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }
}
