using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public int previousColumn;
    public int previousRow;
    public Board board;
    public bool isMatched=false;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float SwipeAngle = 0;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        targetX=(int)transform.position.x;
        targetY=(int)transform.position.y;
        column = targetX;
        row = targetY;
        previousColumn = column;
        previousRow = row;
    }
    private void Update()
    {
        FindMatches();
        if(isMatched)
        {
            SpriteRenderer mySprite=GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, 0.2f);
        }
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition=new Vector2(targetX,transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition,0.4f);
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
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }
    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        SwipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x)*180/Mathf.PI;
        MovePieces();
    }
    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(0.5f);
        if(otherDot!=null)
        {
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().column = column;
                otherDot.GetComponent<Dot>().row = row;
                row = previousRow;
                column= previousColumn;
            }
            otherDot = null;
        }
    }
    void MovePieces()
    {
        if(SwipeAngle>-45 && SwipeAngle<=45 && column<board.width-1)
        {
            //right
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (SwipeAngle > 45 && SwipeAngle <= 135 && row<board.height-1)
        {
            //Up
            otherDot = board.allDots[column , row+1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && column>0)
        {
            //left
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (SwipeAngle >= -135 && SwipeAngle < -45 && row>0)
        {
            //down
            otherDot = board.allDots[column , row-1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }
    void FindMatches()
    {
        if(column>0 && column<board.width-1)
        {
            GameObject leftDot = board.allDots[column-1,row];
            GameObject rightDot = board.allDots[column+1,row];
            if(leftDot.gameObject.tag==gameObject.tag && rightDot.gameObject.tag == gameObject.tag)
            {
                leftDot.GetComponent<Dot>().isMatched = true;
                rightDot.GetComponent<Dot>().isMatched= true;
                isMatched = true;
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject downtDot = board.allDots[column , row - 1];
            GameObject upDot = board.allDots[column , row+1];
            if (downtDot.gameObject.tag == gameObject.tag && upDot.gameObject.tag == gameObject.tag)
            {
                downtDot.GetComponent<Dot>().isMatched = true;
                upDot.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
