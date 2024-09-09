using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column, row, targetX, targetY, previousColumn, previousRow;
    public Board board;
    public HintManager hintManager;
    public bool isMatched = false;

    [Header("Other"), Space(10)]
    private FindMatches findMatches;
    private EndGameManager endGameManager;
    public GameObject otherDot;
    private Vector2 firstTouchPosition, finalTouchPosition, tempPosition;

    [Header("Swipe Stuff")]
    public float SwipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Powerup Stuff"), Space(10)]
    public bool isColumnBomb, isRowBomb, isColorBomb, isAdjacenBomb;
    public GameObject adjacentMaker, rowArrow, columnArrow, colorBomb;

    private void Start()
    {
        InitializeDot();
    }

    private void InitializeDot()
    {
        isColumnBomb = isRowBomb = isColorBomb = isAdjacenBomb = false;
        endGameManager = FindObjectOfType<EndGameManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MakeBomb(colorBomb, ref isColorBomb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MakeBomb(adjacentMaker, ref isAdjacenBomb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MakeBomb(rowArrow, ref isRowBomb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MakeBomb(columnArrow, ref isColumnBomb);
        }
    }

    private void Update()
    {
        UpdateDotPosition();
    }

    public void UpdateDotPosition()
    {
        targetX = column;
        targetY = row;

        MoveTowardsTarget(ref targetX, transform.position.x, Vector2.right);
        MoveTowardsTarget(ref targetY, transform.position.y, Vector2.up);
    }

    private void MoveTowardsTarget(ref int target, float currentPosition, Vector2 direction)
    {
        float targetPosition = target * (direction == Vector2.right ? 1 : (direction == Vector2.left ? -1 : 0)) +
                               (direction == Vector2.up ? target : (direction == Vector2.down ? -target : 0));

        if (Mathf.Abs(targetPosition - currentPosition) > 0.1f)
        {
            // Lerp giữa vị trí hiện tại và vị trí mục tiêu
            tempPosition = direction * (target - currentPosition);
            transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + tempPosition, 0.6f);
            UpdateBoardPosition();
        }
        else
        {
            // Đặt vị trí hiện tại thành vị trí mục tiêu
            transform.position = new Vector2(target, transform.position.y) * (direction == Vector2.right ? 1 : (direction == Vector2.left ? -1 : 0)) +
                                 new Vector2(transform.position.x, target) * (direction == Vector2.up ? 1 : (direction == Vector2.down ? -1 : 0));
        }
    }


    private void UpdateBoardPosition()
    {
        if (board.allDots[column, row] != this.gameObject)
        {
            board.allDots[column, row] = this.gameObject;
            findMatches.FindAllMatches();
        }
    }

    private void OnMouseDown()
    {
        hintManager?.DestroyHint();

        if (board.currentStates == GameStates.Move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentStates = GameStates.Wait;
            SwipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentDot = this;
        }
        else
        {
            board.currentStates = GameStates.Move;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        HandleBomb();
        yield return new WaitForSeconds(0.5f);
        HandleMoveOutcome();
    }
    private void HandleBomb()
    {
        HandleColorBombs();
        HandleOtherBombs();
    }
    private void OnDestroy()
    {
        if(isAdjacenBomb)
        {
            findMatches.MatchAdjancentPieces(column, row);
        }
        if (isColumnBomb)
        {
            findMatches.MatchColumnPieces(column);
        }
        if(isRowBomb)
        {
            findMatches.MatchRowPieces(row);
        }
    }
    //bomb
    private void HandleColorBombs()
    {
        if(isColorBomb && otherDot.GetComponent<Dot>().isColorBomb)
        {
            for(int i = 0; i < board.dots.Length; i++)
            {
                findMatches.MatchPiecesOfColor(board.dots[i].tag);
            }
        }
        else if (isColorBomb)
        {
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }
    }
    private void HandleOtherBombs()
    {
        if (isAdjacenBomb && otherDot.GetComponent<Dot>().isAdjacenBomb)
        {
            findMatches.MatchAdjancentPieces(previousColumn,previousRow);
            findMatches.MatchAdjancentPieces(column,row);
        }

        if (isColumnBomb && otherDot.GetComponent<Dot>().isColumnBomb)
        {
            findMatches.MatchColumnPieces(previousColumn);
            findMatches.MatchColumnPieces(column);
        }

        if (isColumnBomb && otherDot.GetComponent<Dot>().isRowBomb)
        {
            findMatches.MatchColumnPieces(column);
            findMatches.MatchRowPieces(previousRow);
        }
        else if(isRowBomb && otherDot.GetComponent<Dot>().isColumnBomb)
        {
            findMatches.MatchRowPieces(row);
            findMatches.MatchColumnPieces(previousColumn);
        }

        if (isRowBomb && otherDot.GetComponent<Dot>().isRowBomb)
        {
            findMatches.MatchRowPieces(previousRow);
            findMatches.MatchRowPieces(row);
        }

    }

    //bomb
    private void HandleMoveOutcome()
    {
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
               
                SwapDotsBack();
            }
            else
            {
                if (endGameManager != null && endGameManager.endGameRequirements.gameType == GameType.Moves)
                {
                    endGameManager.DecreaseCounterValue();
                }
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }

    private void SwapDotsBack()
    {
        otherDot.GetComponent<Dot>().column = column;
        otherDot.GetComponent<Dot>().row = row;
        row = previousRow;
        column = previousColumn;
        StartCoroutine(ResetStateAfterMove());
    }

    private IEnumerator ResetStateAfterMove()
    {
        yield return new WaitForSeconds(0.5f);
        board.currentDot = null;
        board.currentStates = GameStates.Move;
    }

    public void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousColumn = column;
        previousRow = row;
        if (otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentStates = GameStates.Move;
        }
    }

    void MovePieces()
    {
        if (SwipeAngle > -45 && SwipeAngle <= 45 && column < board.width - 1)
        {
            MovePiecesActual(Vector2.right);
        }
        else if (SwipeAngle > 45 && SwipeAngle <= 135 && row < board.height - 1)
        {
            MovePiecesActual(Vector2.up);
        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && column > 0)
        {
            MovePiecesActual(Vector2.left);
        }
        else if (SwipeAngle >= -135 && SwipeAngle < -45 && row > 0)
        {
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentStates = GameStates.Move;
        }
    }

    //void FindMatches()
    //{
    //    CheckForMatch(Vector2.left, Vector2.right);
    //    CheckForMatch(Vector2.up, Vector2.down);
    //}

    //private void CheckForMatch(Vector2 direction1, Vector2 direction2)
    //{
    //    GameObject dot1 = GetDotInDirection(direction1);
    //    GameObject dot2 = GetDotInDirection(direction2);

    //    if (dot1 != null && dot2 != null && dot1.tag == gameObject.tag && dot2.tag == gameObject.tag)
    //    {
    //        dot1.GetComponent<Dot>().isMatched = true;
    //        dot2.GetComponent<Dot>().isMatched = true;
    //        isMatched = true;
    //    }
    //}

    //private GameObject GetDotInDirection(Vector2 direction)
    //{
    //    int targetColumn = column + (int)direction.x;
    //    int targetRow = row + (int)direction.y;

    //    if (targetColumn >= 0 && targetColumn < board.width && targetRow >= 0 && targetRow < board.height)
    //    {
    //        return board.allDots[targetColumn, targetRow];
    //    }
    //    return null;
    //}

    private void MakeBomb(GameObject bombPrefab, ref bool bombFlag)
    {
        if (!isColumnBomb && !isRowBomb && !isColorBomb && !isAdjacenBomb)
        {
            bombFlag = true;
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.transform.parent = transform;
        }
    }

    public void MakeRowBomb() => MakeBomb(rowArrow, ref isRowBomb);
    public void MakeColumnBomb() => MakeBomb(columnArrow, ref isColumnBomb);
    public void MakeColorBomb() => MakeBomb(colorBomb, ref isColorBomb);
    public void MakeAdjacenBomb() => MakeBomb(adjacentMaker, ref isAdjacenBomb);
}
