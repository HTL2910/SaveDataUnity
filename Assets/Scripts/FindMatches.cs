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
    private List<GameObject> IsAdjacenBomb(Dot dot1,Dot dot2,Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isAdjacenBomb)
        {
            currentMatches.Union(GetAdjancentPieces(dot1.column,dot1.row));
            
        }
        if (dot2.isAdjacenBomb)
        {
            currentMatches.Union(GetAdjancentPieces(dot2.column, dot2.row));
        }
        if (dot3.isAdjacenBomb)
        {
            currentMatches.Union(GetAdjancentPieces(dot3.column, dot3.row));
        }
        return currentDots;
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
        yield return new WaitForSeconds(0.3f);
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
                                    currentMatches.Union(IsAdjacenBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearByPieces(leftDot, currentDot, rightDot);
                                    //UIManager.Instance.PlaySound(UIManager.Instance.audioclipgood);
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
                                    currentMatches.Union(IsAdjacenBomb(upDotDot, currentDotDot, downDotDot));

                                    GetNearByPieces(upDot, currentDot, downDot);
                                    //UIManager.Instance.PlaySound(UIManager.Instance.audioclipgood);
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
        //UIManager.Instance.PlaySound(UIManager.Instance.audioclipBoomColor);
    }    
    List<GameObject> GetAdjancentPieces(int column,int  row)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i=column-1;i<=column+1;i++)
        {
            for(int j=row-1;j<=row+1;j++)
            {
                if (i >= 0 && i<board.width && j>=0 && j<board.height)
                {
                    dots.Add(board.allDots[i, j]);
                    board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                }
            }    
            
        }
        return dots;
    }
    private List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i=0;i<board.height;i++)
        {
            if (board.allDots[column,i]!=null)
            {
                Dot dot = board.allDots[column, i].GetComponent<Dot>();
                if (dot.isRowBomb)
                {
                    dots.Union(GetRowPieces(i).ToList());
                }
                dots.Add(board.allDots[column, i]);
                dot.isMatched = true;
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
                Dot dot = board.allDots[i, row].GetComponent<Dot>();
                if (dot.isColumnBomb)
                {
                    dots.Union(GetColumnPieces(i).ToList());
                }
                dots.Add(board.allDots[i, row]);
                dot.isMatched = true;
            }
        }

        return dots;
    }
    //public void CheckBombs()
    //{
    //    if (board.currentDot != null)
    //    {
    //        if (board.currentDot.isMatched)
    //        {
    //            board.currentDot.isMatched = false;
    //            //cach 1
    //            //int typeOfBomb = Random.Range(0, 100);
    //            //if(typeOfBomb<50)
    //            //{
    //            //    //row bomd
    //            //    board.currentDot.MakeRowBomb();
    //            //}    
    //            //else if (typeOfBomb >= 50)
    //            //{
    //            //    //column bomb
    //            //    board.currentDot.MakeColumnBomb();
    //            //}
    //            //cach 2:
    //            if ((board.currentDot.SwipeAngle > -45f && board.currentDot.SwipeAngle <= 45)
    //               || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
    //            {
    //                board.currentDot.MakeRowBomb();
    //                //UIManager.Instance.PlaySound(UIManager.Instance.audioclipBoom);
    //            }
    //            else
    //            {
    //                board.currentDot.MakeColumnBomb();
    //                //UIManager.Instance.PlaySound(UIManager.Instance.audioclipBoom);
    //            }
    //        }
    //    }
    //    else if (board.currentDot.otherDot != null)
    //    {
    //        Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
    //        if (otherDot.isMatched)
    //        {
    //            otherDot.isMatched = false;
    //            //cach 1
    //            //int typeOfBomb = Random.Range(0, 100);
    //            //if (typeOfBomb < 50)
    //            //{
    //            //    //row bomd
    //            //    otherDot.MakeRowBomb();
    //            //}
    //            //else if (typeOfBomb >= 50)
    //            //{
    //            //    //column bomb
    //            //    otherDot.MakeColumnBomb();
    //            //}
    //            //cach 2:
    //            if ((board.currentDot.SwipeAngle > -45f && board.currentDot.SwipeAngle <= 45)
    //                || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
    //            {
    //                otherDot.MakeRowBomb();
    //                //UIManager.Instance.PlaySound(UIManager.Instance.audioclipBoom);
    //            }
    //            else
    //            {
    //                otherDot.MakeColumnBomb();
    //                //UIManager.Instance.PlaySound(UIManager.Instance.audioclipBoom);
    //            }

    //        }
    //    }
    //}
}
