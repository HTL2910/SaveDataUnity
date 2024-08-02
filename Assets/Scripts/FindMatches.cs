using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        board=FindObjectOfType<Board>();
    }
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    private List<GameObject> IsRowBomd(Dot dot1,Dot dot2,Dot dot3)
    {
        List<GameObject> currentDots=new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }
        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }
        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }
        return currentDots;
    }
    private List<GameObject> IsColumnBomb(Dot dot1,Dot dot2,Dot dot3)
    {
        List<GameObject> currentDots=new List<GameObject>();
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }
        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }
        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }
        return currentDots;
    }
    private void AddToListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().isMatched = true;
    }
    private void GetNearByPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for(int i=0;i<board.width;i++)
        {
            for(int j=0;j<board.height;j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if (i>0 && i<board.width-1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            if (leftDot != null && rightDot != null)
                            {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                                {
                                    currentMatches.Union(IsRowBomd(leftDotDot, currentDotDot, rightDotDot));
                                    currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearByPieces(leftDot, currentDot, rightDot);
                                }
                            }
                        } 
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i , j+1];
                        GameObject downDot = board.allDots[i , j-1];
                        if(upDot!=null && downDot != null)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();
                            if (upDot != null && downDot != null)
                            {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                                {

                                    currentMatches.Union(IsRowBomd(upDotDot, currentDotDot, downDotDot));
                                    currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                                    GetNearByPieces(upDot, currentDot, downDot);
                                }
                            }
                        }
                        
                    }
                }
            }
        }
    }
    public void MatchPiecesOfColor(string color)
    {
        for(int i=0;i<board.width;i++)
        {
            for(int j=0;j<board.height;j++)
            {
                if (board.allDots[i,j]!=null)
                {
                    if (board.allDots[i,j].tag==color)
                    {
                        board.allDots[i,j].GetComponent<Dot>().isMatched = true;    
                    }    
                }    
            }    
        }
    }    
    private List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i=0;i<board.height;i++)
        {
            if (board.allDots[column,i]!=null)
            {
                dots.Add(board.allDots[column, i]);
                board.allDots[column,i].GetComponent<Dot>().isMatched = true;
            }    
        }    

        return dots;
    }
    private List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }

        return dots;
    }
    public void CheckBombs()
    {
        if (board.currentDot != null)
        {
            if (board.currentDot.isMatched)
            {
                board.currentDot.isMatched = false;
                //cach 1
                //int typeOfBomb = Random.Range(0, 100);
                //if(typeOfBomb<50)
                //{
                //    //row bomd
                //    board.currentDot.MakeRowBomb();
                //}    
                //else if (typeOfBomb >= 50)
                //{
                //    //column bomb
                //    board.currentDot.MakeColumnBomb();
                //}
                //cach 2:
                if ((board.currentDot.SwipeAngle > -45f && board.currentDot.SwipeAngle <= 45)
                   || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
                {
                    board.currentDot.MakeRowBomb();
                }
                else
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
        }
        else if (board.currentDot.otherDot != null)
        {
            Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
            if (otherDot.isMatched)
            {
                otherDot.isMatched = false;
                //cach 1
                //int typeOfBomb = Random.Range(0, 100);
                //if (typeOfBomb < 50)
                //{
                //    //row bomd
                //    otherDot.MakeRowBomb();
                //}
                //else if (typeOfBomb >= 50)
                //{
                //    //column bomb
                //    otherDot.MakeColumnBomb();
                //}
                //cach 2:
                if ((board.currentDot.SwipeAngle > -45f && board.currentDot.SwipeAngle <= 45)
                    || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
                {
                    otherDot.MakeRowBomb();
                }
                else
                {
                    otherDot.MakeColumnBomb();
                }

            }
        }
    }
}
