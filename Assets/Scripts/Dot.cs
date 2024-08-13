using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public int previousColumn;
    public int previousRow;
    public Board board;
    public HintManager hintManager;
    public bool isMatched=false;
    [Header("Other"),Space(10)]
    private FindMatches findMatches;
    public GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    [Header("Swipe Stuff")]
    public float SwipeAngle = 0;
    public float swipeResist = 1f;
    [Header("Powerup Stuff"), Space(10)]
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isColorBomb;
    public bool isAdjacenBomb;
    public GameObject adjacentMaker;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;
    private void Start()
    {
        isColumnBomb=false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacenBomb = false;

        hintManager=FindObjectOfType<HintManager>();
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX=(int)transform.position.x;
        //targetY=(int)transform.position.y;
        //column = targetX;
        //row = targetY;
        //previousColumn = column;
        //previousRow = row;
    }
    //testing
    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        isColumnBomb = true;
    //        GameObject arrow=Instantiate(columnArrow,transform.position,Quaternion.identity);
    //        arrow.transform.parent = transform;
    //    }
    //    if (Input.GetMouseButtonDown(2))
    //    {
    //        isRowBomb = true;
    //        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
    //        arrow.transform.parent = transform;
    //    }
    //}
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAdjacenBomb = true;
            GameObject maker = Instantiate(adjacentMaker, transform.position, Quaternion.identity);
            maker.transform.parent = transform;
        }
    }
    private void Update()
    {
        //if(isMatched)
        //{
        //    SpriteRenderer mySprite=GetComponent<SpriteRenderer>();
        //    mySprite.color = new Color(0f, 0f, 0f, 0.2f);
        //}
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition=new Vector2(targetX,transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition,0.6f);
            if (board.allDots[column,row]!=this.gameObject)
            {
                board.allDots[column,row]= this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }
    private void OnMouseDown()
    {
        if(hintManager != null)
        {
            hintManager.DestroyHint();
        }
        
        if (board.currentStates == GameStates.Move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(firstTouchPosition);
        }
      
    }
    private void OnMouseUp()
    {
        if (board.currentStates == GameStates.Move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y-firstTouchPosition.y)>swipeResist||
            Mathf.Abs(finalTouchPosition.x-firstTouchPosition.x)>swipeResist)
        {
            board.currentStates = GameStates.Wait;
            SwipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            
            board.currentDot = this;
        }
        else
        {
            board.currentStates=GameStates.Move;
           
        }
    }
    public IEnumerator CheckMoveCo()
    {
        if (isColorBomb)
        {
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched= true;
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }
        yield return new WaitForSeconds(0.5f);
        if(otherDot!=null)
        {
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().column = column;
                otherDot.GetComponent<Dot>().row = row;
                row = previousRow;
                column= previousColumn;
                UIManager.Instance.PlaySound(UIManager.Instance.audioclipFall);
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null; 
                board.currentStates = GameStates.Move;
                
            }
            else
            {
                board.DestroyMatches();
                UIManager.Instance.PlaySound(UIManager.Instance.audioclipgood);

            }
            otherDot = null;
        }
        
    }
    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column +(int)direction.x, row+(int)direction.y];
        previousColumn = column;
        previousRow = row;
        if(otherDot!=null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentStates=GameStates.Move;
        }        
    }    
    void MovePieces()
    {
        if(SwipeAngle>-45 && SwipeAngle<=45 && column<board.width-1)
        {
            ////right
            //otherDot = board.allDots[column + 1, row];
            //previousColumn = column;
            //previousRow = row;
            //otherDot.GetComponent<Dot>().column -= 1;
            //column += 1;
            //StartCoroutine(CheckMoveCo());
            MovePiecesActual(Vector2.right);
        }
        else if (SwipeAngle > 45 && SwipeAngle <= 135 && row<board.height-1)
        {
            //Up
            //otherDot = board.allDots[column , row+1];
            //previousColumn = column;
            //previousRow = row;
            //otherDot.GetComponent<Dot>().row -= 1;
            //row += 1;
            //StartCoroutine(CheckMoveCo());
            MovePiecesActual(Vector2.up);

        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && column>0)
        {
            //left
            //otherDot = board.allDots[column - 1, row];
            //previousColumn = column;
            //previousRow = row;
            //otherDot.GetComponent<Dot>().column += 1;
            //column -= 1;
            //StartCoroutine(CheckMoveCo());
            MovePiecesActual(Vector2.left);

        }
        else if (SwipeAngle >= -135 && SwipeAngle < -45 && row>0)
        {
            //down
            //otherDot = board.allDots[column , row-1];
            //previousColumn = column;
            //previousRow = row;
            //otherDot.GetComponent<Dot>().row += 1;
            //row -= 1;
            //StartCoroutine(CheckMoveCo());
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentStates = GameStates.Move;
        }
    }
    void FindMatches()
    {
        if(column>0 && column<board.width-1)
        {
            GameObject leftDot = board.allDots[column-1,row];
            GameObject rightDot = board.allDots[column+1,row];
            if(leftDot!=null && rightDot!=null)
            {
                if (leftDot.gameObject.tag == gameObject.tag && rightDot.gameObject.tag == gameObject.tag)
                {
                    leftDot.GetComponent<Dot>().isMatched = true;
                    rightDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
            
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject downtDot = board.allDots[column , row - 1];
            GameObject upDot = board.allDots[column , row+1];
            if(downtDot!=null && upDot!=null)
            {
                if (downtDot.gameObject.tag == gameObject.tag && upDot.gameObject.tag == gameObject.tag)
                {
                    downtDot.GetComponent<Dot>().isMatched = true;
                    upDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
            
        }
    }
    public void MakeRowBomb()
    {
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }    
    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = transform;
    }
    public void MakeColorBomb()
    {
        isColorBomb = true;
        GameObject color=Instantiate(colorBomb, transform.position, Quaternion.identity);   
        color.transform.parent = transform;
    }
    public void MakeAdjacenBomb()
    {
        isAdjacenBomb = true;
        GameObject maker = Instantiate(adjacentMaker, transform.position, Quaternion.identity);
        maker.transform.parent = transform;
    }
}
