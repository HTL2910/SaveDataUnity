using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameStates
{
    Wait,
    Move,
    Win,
    Lose,
    Pause
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
    [Header("Scriptsable Object Stuff")]
    public World world;
    public int level;

    private FindMatches findMatches;
    public GameStates currentStates = GameStates.Move;
    [Header("Board Dimensions")]
    //public GameObject[] alldotsUse;
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
    private ScoreManager scoreManager;
    private GoalsManager goalsManager;
    public int basePieceValue = 20;
    private int streakValue=1;
    public float refillDelay = 0.5f;

    public int[] scoreGoal;

    public TextMeshProUGUI levelText;
    public GameObject panelItem;
    #endregion
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }
        levelText.text = "LEVEL: "+(level+1).ToString();
        if (world != null)
        {
            if (level < world.levels.Length)
            {
                if (world.levels[level] != null)
                {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    scoreGoal = world.levels[level].scoreGoal;
                    boardLayout = world.levels[level].boardLayout;

                }
            }
        }
    }
   
    private void Start()
    {
        breakableTiles=new BackgroundTitle[width,height];
        findMatches=FindObjectOfType<FindMatches>();
        goalsManager = FindObjectOfType<GoalsManager>();
        scoreManager=FindObjectOfType<ScoreManager>();
        blankSpaces = new bool[width, height];
        allDots= new GameObject[width, height];
        SetUp();
        panelItem.SetActive(false);
        currentStates = GameStates.Pause;
    }
   
    public void GenerateBlankSpaces()//Start
    {
        for(int i=0;i<boardLayout.Length;i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }
    public void GenerateBreakableTiles()//Start
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);

                GameObject tile=Instantiate(breakableTilePrefabs,tempPosition,Quaternion.identity);
                tile.transform.parent = this.transform;
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTitle>();
            }
        }
    }
    private void InitBoardGame()//Start
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSets);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backGroundTiles = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
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
    private void SetUp()//Start
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        InitBoardGame();
    }

   
    //check 3 dot similar in row and column
    private bool MatchesAt(int column, int row, GameObject piece)//Start
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
    //check to create bomb col and color adjacent
    private int ColumnOrRow()
    {
        List<GameObject> matchCopy=findMatches.currentMatches as List<GameObject>;
        for(int i = 0; i < matchCopy.Count; i++)
        {
            Dot thisDot = matchCopy[i].GetComponent<Dot>();
            int column = thisDot.column;
            int row=thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;

            for(int j = 0; j < matchCopy.Count; j++)
            {
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if (nextDot == thisDot)
                {
                    continue;
                }
                if(nextDot.column==thisDot.column &&
                    nextDot.CompareTag(thisDot.tag))
                {
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row &&
                   nextDot.CompareTag(thisDot.tag))
                {
                    rowMatch++;
                }
            }
            //return 3 if column and row match
            //return 2 if adjacent
            //return 1 if color bomb
            if(columnMatch==4 || rowMatch == 4)
            {
                return 1;
            }
            if(columnMatch==2 && rowMatch == 2)
            {
                return 2;
            }
            if(columnMatch == 3 || rowMatch == 3)
            {
                return 3;
            }

        }
        
        return 0;
        #region
        //int numberHorizontal = 0;
        //int numberVertical = 0;
        //Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        //if (firstPiece != null)
        //{
        //    foreach(GameObject currentPieces in findMatches.currentMatches)
        //    {
        //        Dot dot = currentPieces.GetComponent<Dot>();
        //        if (dot.row == firstPiece.row)
        //        {
        //            numberHorizontal++;
        //        }
        //        if(dot.column==firstPiece.column)
        //        {
        //            numberVertical++;
        //        }    
        //    }    
        //}
        //return (numberVertical == 5 || numberHorizontal == 5);
        #endregion
    }
   
    //Make bomb
    private void CheckToMakeBombs()//Kiểm tra để tạo bomb nào
    {
        if (findMatches.currentMatches.Count > 3)
        {
            int typeOfMatch = ColumnOrRow();
            if (typeOfMatch == 1)
            {
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
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
                                if (!otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else if (typeOfMatch == 2)
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
        
            else if (typeOfMatch == 3)
            {
                //findMatches.CheckBombs();
                int indexRan = Random.Range(0, 2);
                currentDot.isMatched = false;
                if (indexRan == 0)
                {
                    
                    currentDot.MakeColumnBomb();
                }
                else
                {
                    currentDot.MakeRowBomb();
                }
            }
        }
        #region
        //if(findMatches.currentMatches.Count==4|| findMatches.currentMatches.Count==7)
        //{
        //    findMatches.CheckBombs();

        //}   
        //if(findMatches.currentMatches.Count==5 || findMatches.currentMatches.Count == 8)
        //{
        //    if(ColumnOrRow())
        //    {
        //        if(currentDot!=null)
        //        {
        //            if(currentDot.isMatched)
        //            {
        //                if (!currentDot.isColorBomb)
        //                {
        //                    currentDot.isMatched = false;
        //                    currentDot.MakeColorBomb();
        //                }
        //            }
        //            else
        //            {
        //                if (currentDot.otherDot != null)
        //                {
        //                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
        //                    if (otherDot.isMatched)
        //                    {
        //                        if(!otherDot.isColorBomb)
        //                        {
        //                            otherDot.isMatched = false;
        //                            otherDot.MakeColorBomb();
        //                        }    
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        if (currentDot != null)
        //        {
        //            if (currentDot.isMatched)
        //            {
        //                if (!currentDot.isAdjacenBomb)
        //                {
        //                    currentDot.isMatched = false;
        //                    currentDot.MakeAdjacenBomb();
        //                }
        //            }
        //            else
        //            {
        //                if (currentDot.otherDot != null)
        //                {
        //                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
        //                    if (otherDot.isMatched)
        //                    {
        //                        if (!otherDot.isAdjacenBomb)
        //                        {
        //                            otherDot.isMatched = false;
        //                            otherDot.MakeAdjacenBomb();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
    }
   
    private void DestroyMatchesAt(int column,int row)//Xóa Dot ơ vị tri col and row
    {
       
        if (allDots[column,row].GetComponent<Dot>().isMatched)
        {
            CheckToMakeBombs();
            
            if (breakableTiles[column,row]!= null)
            {
                breakableTiles[column, row].TakeDamage(1);
                if(breakableTiles[column, row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }
            if (goalsManager != null)
            {
                goalsManager.CompareGoal(allDots[column, row].tag);
                goalsManager.UpdateGoal();
            }
            GameObject particle=Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            UIManager.Instance.PlaySound(UIManager.Instance.audioclipgood);
            Destroy(particle, 0.5f);
            Destroy(allDots[column,row]);
            scoreManager.IncreaseScore(basePieceValue*streakValue);
            allDots[column,row] = null;
        }
        else { }
    }
    public void DestroyMatches()//xóa dot đó sau khi check có match
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
        yield return new WaitForSeconds(refillDelay*0.5f);
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
                    int maxIterations = 0;
                    while (MatchesAt(i, j, dots[dotToUse])&& maxIterations<100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    maxIterations = 0;
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
        yield return new WaitForSeconds(refillDelay);
        RefillBoard();
        while(MatchesInBoard())
        {
            streakValue += 1;
            findMatches.FindAllMatches();
            DestroyMatches();
            yield return new WaitForSeconds(2*refillDelay);
           
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(refillDelay);
        if (IsDeadLocked())
        {
            StartCoroutine(ShuffleBoard());
        }
        streakValue = 1;
        currentStates = GameStates.Move;

        findMatches.FindAllMatches();
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
    public bool SwitchAndCheck(int column,int row,Vector2 direction)
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
    public IEnumerator ShuffleBoard()
    {
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(0.5f);

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
            StartCoroutine(ShuffleBoard());
        }
    }
    #region
    //code Button
    private Vector2Int GetRandomPositionWithoutBomb()
    {
        int ranWidth = Random.Range(0, width);
        int ranHeight = Random.Range(0, height);
        int attempts = 0;

        while ((allDots[ranWidth, ranHeight].GetComponent<Dot>().isAdjacenBomb ||
                allDots[ranWidth, ranHeight].GetComponent<Dot>().isColorBomb ||
                allDots[ranWidth, ranHeight].GetComponent<Dot>().isColumnBomb ||
                allDots[ranWidth, ranHeight].GetComponent<Dot>().isRowBomb) && attempts < 100)
        {
            ranWidth = Random.Range(0, width);
            ranHeight = Random.Range(0, height);
            attempts++;
        }

        if (attempts >= 100)
        {
            Debug.LogWarning("Không thể tìm thấy vị trí nào không chứa bom sau 100 lần thử.");
        }

        return new Vector2Int(ranWidth, ranHeight);
    }

    public void CreateColorBomb()
    {
        Vector2Int position = GetRandomPositionWithoutBomb();
        allDots[position.x, position.y].GetComponent<Dot>().MakeColorBomb();
    }

    public void CreateAdjacenBomb()
    {
        Vector2Int position = GetRandomPositionWithoutBomb();
        allDots[position.x, position.y].GetComponent<Dot>().MakeAdjacenBomb();
    }

    public void CreateColumnBomb()
    {
        Vector2Int position = GetRandomPositionWithoutBomb();
        allDots[position.x, position.y].GetComponent<Dot>().MakeColumnBomb();
    }

    public void CreateRowBomb()
    {
        Vector2Int position = GetRandomPositionWithoutBomb();
        allDots[position.x, position.y].GetComponent<Dot>().MakeRowBomb();
    }

    #endregion
    
    public void ActivePanelItem()
    {
        panelItem.SetActive(true);
        StartCoroutine(DeactivePanelItem());
    }
    public IEnumerator DeactivePanelItem()
    {
        yield return new WaitForSeconds(1f);
        panelItem.SetActive(false);
    }
}
