using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStates
{
    Wait,
    Move
}
public class Board : MonoBehaviour
{
    private FindMatches findMatches;
    public GameStates currentStates = GameStates.Move;
    public GameObject[] dots;
    public int width;
    public int height;
    public int offSets;
    public GameObject tilePrefab;
    private BackgroundTitle[,] allTiles;
    public GameObject[,] allDots;
    private void Start()
    {
        findMatches=FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTitle[width, height];
        allDots= new GameObject[width, height];
        SetUp();
    }
    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j+offSets);
                GameObject backGroundTiles=Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backGroundTiles.transform.parent=this.transform;
                backGroundTiles.name="("+i+","+j+")";
                int dotToUse = Random.Range(0, dots.Length);
                int maxInteration = 0;
                while (MatchesAt(i, j, dots[dotToUse]) && maxInteration<100)
                {
                    dotToUse=Random.Range(0,dots.Length);
                    maxInteration++;
                }
                maxInteration = 0;
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;
                
                dot.transform.parent = transform;
                dot.name = "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }
    }
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column,row-1].tag==piece.tag && allDots[column,row-2].tag==piece.tag)
            {
                {
                    return true;
                } 
            }
        }
        else if(column<=1 || row<=1)
        {
            if (row > 1)
            {
                if (allDots[column,row-1].tag==piece.tag && allDots[column,row-2].tag==piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column-1, row ].tag == piece.tag && allDots[column-2, row ].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void DestroyMatchesAt(int column,int row)
    {
        if (allDots[column,row].GetComponent<Dot>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column,row]);
            allDots[column,row] = null;
        }
    }
    public void DestroyMatches()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height; j++)
            {
                if (allDots[i, j]== null)
                {
                    nullCount++;
                }    
                else if(nullCount >0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }   
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if (allDots[i,j]== null)
                {
                    Vector2 tempPos = new Vector2(i, j+offSets);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }    
            }    
        }    
    }
    private bool MatchesInBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i,j]!=null)
                {
                    if (allDots[i,j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }    
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(0.5f);
        while(MatchesInBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(0.5f);
        currentStates = GameStates.Move;
    }    
}
