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
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        board=FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void IncreaseScore(int amoutToIncrease)
    {
        score += amoutToIncrease;
        if(board!=null && scorebarImage!=null)
        {
            int length=board.scoreGoal.Length;
            scorebarImage.fillAmount = (float)score / (float)board.scoreGoal[length-1];
        }
    }
}
