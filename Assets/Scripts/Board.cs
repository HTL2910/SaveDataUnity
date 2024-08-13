using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStates
{
    Wait,
    Move
}
public enum TileKind
{
    Breakable,
    Blank,
    Normal
}
[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    private FindMatches findMatches;
    public GameStates currentStates = GameStates.Move;
    public GameObject[] dots;
    #region
    public int width;
    public int height;
    public int offSets;
    public GameObject destroyEffect;
    public GameObject tilePrefab;
    public TileType[] boardLayout;
    private bool[,] blankSpaces;
    public GameObject[,] allDots;
    public Dot currentDot;
    public BackgroundTitle[,] breakableTiles;
    public GameObject breakableTilePrefabs;
    #endregion
    private void Start()
    {
        breakableTiles=new BackgroundTitle[width,height];
        findMatches=FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allDots= new GameObject[width, height];
        SetUp();
    }
    public void GenerateBlankSpaces()
    {
        for(int i=0;i<boardLayout.Length;i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }
    public void GenerateBreakableTiles()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);

                GameObject tile=Instantiate(breakableTilePrefabs,tempPosition,Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTitle>();
            }
        }
    }
    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i,j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSets);
                    GameObject backGroundTiles = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    backGroundTiles.transform.parent = this.transform;
                    backGroundTiles.name = "(" + i + "," + j + ")";
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxInteration = 0;
                    while (MatchesAt(i, j, dots[dotToUse]) && maxInteration < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
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
    }
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column-1,row]!=null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)

                {
                    return true;
                }
            }
            
        }
        else if(column<=1 || row<=1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private bool ColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null)
        {
            foreach(GameObject currentPieces in findMatches.currentMatches)
            {
                Dot dot = currentPieces.GetComponent<Dot>();
                if (dot.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if(dot.column==firstPiece.column)
                {
                    numberVertical++;
                }    
            }    
        }
        return (numberVertical == 5 || numberHorizontal == 5);
    }
    private void CheckToMakeBombs()
    {
        if(findMatches.currentMatches.Count==4|| findMatches.currentMatches.Count==7)
        {
            findMatches.CheckBombs();

        }   
        if(findMatches.currentMatches.Count==5 || findMatches.currentMatches.Count == 8)
        {
            if(ColumnOrRow())
            {
                if(currentDot!=null)
                {
                    if(currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if(!otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }    
                            }
                        }
                    }
                }
              
            }
            else
            {
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacenBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacenBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isAdjacenBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacenBomb();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void DestroyMatchesAt(int column,int row)
    {
        if (allDots[column,row].GetComponent<Dot>().isMatched)
        {
            //
            if(findMatches.currentMatches.Count>=4)
            {
                CheckToMakeBombs();
            }
            if (breakableTiles[column,row]!= null)
            {
                breakableTiles[column, row].TakeDamage(1);
                if(breakableTiles[column, row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }    
            GameObject particle=Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 0.5f);
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
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }
    private IEnumerator DecreaseRowCo2()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if (!blankSpaces[i,j] && allDots[i, j] == null)
                {
                    for(int k=j+1;k<height;k++)
                    {
                        if (allDots[i,k]!= null)
                        {
                            allDots[i, k].GetComponent<Dot>().row = j;
                            allDots[i, k] = null;
                            break;
                        }    
                    }    
                }
            }    
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
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
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if (allDots[i,j]== null && !blankSpaces[i,j])
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
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(0.5f);
        if (IsDeadLocked())
        {
            ShuffleBoard();
        }
        currentStates = GameStates.Move;
    }    
    private void SwitchPieces(int column,int row,Vector2 direction)
    {
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y];
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];

        allDots[column,row]= holder;
    }
    private bool CheckForMatches()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width -2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag &&
                                allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2)
                    {
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag &&
                                allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
               
            }    
        }
        return false;
    }
    private bool SwitchAndCheck(int column,int row,Vector2 direction)
    {
        SwitchPieces(column,row,direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row,direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }
    private bool IsDeadLocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if(SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    private void ShuffleBoard()
    {
        List<GameObject> newBoard = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    int maxInteration = 0;
                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxInteration < 100)
                    {
                        pieceToUse = Random.Range(0,newBoard.Count);
                        maxInteration++;
                    }
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                    maxInteration = 0;
                    piece.column = i;
                    piece.row = j;
                    allDots[i, j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }
        if (IsDeadLocked())
        {
            ShuffleBoard();
        }
    }
}
