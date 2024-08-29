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
    public TextMeshProUGUI goalTextResult;
    public TextMeshProUGUI starTextResult;
    int levelScore = 0;
    public int score;
    public int indexLevel=1;
    // Start is called before the first frame update
    void Start()
    {
        board=FindObjectOfType<Board>();
        goalTextResult.text= "Last Target Score: "+board.scoreGoal[2].ToString();
        starTextResult.text = (indexLevel-1).ToString();
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

            if (indexLevel > 3)
            {
                
                starTextResult.text = 3.ToString();
                scorebarImage.fillAmount = 1f;
            }
            else
            {
                scorebarImage.fillAmount = (float)(score - levelScore) / (float)(board.scoreGoal[indexLevel - 1] - levelScore);
                if (scorebarImage.fillAmount >= 1)
                {
                    int randombar = Random.Range(0, barSprites.Count);
                    barGoal.GetComponent<Image>().sprite = barSprites[randombar];

                    scorebarImage.fillAmount = 0f;

                    indexLevel++;
                }
                starTextResult.text = (indexLevel - 1).ToString();
                
            }
            levelScore = score;
        }
    }
}
