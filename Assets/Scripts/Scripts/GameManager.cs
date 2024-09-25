using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class GameData
{
    public int totalLevel;
    public int totalScore;
    public int countColorBomb;
    public int countAdjacenBomb;
    public int countColumnBomb;
    public int countRowBomb;
    public int unclockLevel;
    public int pageIndex;
    public List<LevelData> levels = new List<LevelData>();
    // stores the unlocked level

    // Constructor to initialize with a specified number of levels
    public GameData(int totalLevels, int unclockLevel)
    {
        this.unclockLevel = unclockLevel;
        this.totalLevel = totalLevels;
        for (int i = 0; i < totalLevel; i++)
        {
            LevelData newLevel = new LevelData(i + 1);
            levels.Add(newLevel);
        }
      
        this.pageIndex = 1;
        this.totalScore = 0;
        this.countAdjacenBomb = 0;
        this.countColumnBomb = 0;
        this.countRowBomb = 0;
        this.countColorBomb = 0;
    }

    // Method to check if a level is unlocked
    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber <= totalLevel && levelNumber <= unclockLevel;
    }
}

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public int score;
    public int stars;

    // Constructor for initializing each level
    public LevelData(int levelNumber)
    {
        this.levelNumber = levelNumber;
        this.score = 0;
        this.stars = 0;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    private string saveFilePath;
    void Awake()
    {
        // Đảm bảo chỉ có một instance của GameManager tồn tại
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Không phá hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

       
        gameData = LoadGameData();
        SaveGameData();

    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Back();
                return;
            }
        }
    }
    void Back()
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex;
        if (indexScene<=0)
        {
            Application.Quit();
        }
        else
        {
            Debug.Log("Active Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
    }
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game data saved!"+ saveFilePath);
    }

    public GameData LoadGameData()
    {
        GameData newgameData;
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            newgameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game data loaded!"+saveFilePath);
        }
        else
        {

            // Initialize game data if no save exists
            newgameData = new GameData(500, 1); // 500 levels, starting with level 1 unlocked
            Debug.Log("New game data created!");
        
        }
        return newgameData;
    }
}
