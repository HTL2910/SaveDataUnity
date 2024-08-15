using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public TextMeshProUGUI scoreText;
    public Image scorebarImage;
    public GameObject barGoal;
    public List<Sprite> barSprites;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goalTextResult;
    int levelScore = 0;
    public int score;
    public int indexLevel=1;
    // Start is called before the first frame update
    void Start()
    {
        board=FindObjectOfType<Board>();
        goalTextResult.text= "Last Goal: "+board.scoreGoal[board.scoreGoal.Length-1].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        levelText.text = "Level: " + indexLevel.ToString();
    }

    public void IncreaseScore(int amoutToIncrease)
    {
        score += amoutToIncrease;
        if(board!=null && scorebarImage!=null)
        {
            int length=board.scoreGoal.Length;
            scorebarImage.fillAmount =  (float)(score-levelScore) / (float)(board.scoreGoal[indexLevel-1]-levelScore);
            if (scorebarImage.fillAmount >= 1)
            {
                int randombar=Random.Range(0, barSprites.Count);
                barGoal.GetComponent<Image>().sprite = barSprites[randombar];
                scorebarImage.fillAmount = 0f;
                levelScore = score;
                indexLevel++;
                StartCoroutine(board.ShuffleBoard());
                //Debug.Log("levelScore: " + levelScore + ":" + "score:" + score);
            }
        }
    }
}
