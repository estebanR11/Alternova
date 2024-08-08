using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class BlockList
{
    public List<Block> blocks;
}

[System.Serializable]
public class GameResult
{
    public int total_clicks;
    public int total_time;
    public int pairs;
    public int score;
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = "GameManager)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    [SerializeField] private BlockList blockList;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private AudioManager audioManager;
    public GameStatus status = GameStatus.Playing;
    private int minXSize = 2;
    private int minYSize = 2;
    private int maxXSize = 8;
    private int maxYSize = 8;
    private int pairsFound;
    private int totalPairs;
    private int totalClicks = 0;
    private int gridSizeX;
    private int gridSizeY;
    private float startTime;
    private float endTime;

    public UnityEvent OnStartGame;
    public UnityEvent OnFinishGame;

    BlockController firstSelected = null;

    private string playerName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnStartGame.AddListener(StartGame);
        OnFinishGame.AddListener(EndGame);
    }

    public void StartGame()
    {
        totalClicks = 0;
        pairsFound = 0;
        startTime = Time.time;
        LoadBlocks();
        CalculateGridSize();
        audioManager.GameMusic();
       
    }

    public void LoadBlocks()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("blocks");
        blockList = JsonUtility.FromJson<BlockList>(jsonText.text);
        totalPairs = blockList.blocks.Count / 2;
        UIManager.Instance.Setup(totalPairs);
        if(totalPairs<3)
        {
     
            UIManager.Instance.RestartGame();
            return;
        }
    }

    void CalculateGridSize()
    {
        gridSizeX = blockList.blocks.Max(b => b.C);
        gridSizeY = blockList.blocks.Max(b => b.R);
      
        gridContainer.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridContainer.GetComponent<GridLayoutGroup>().constraintCount = gridSizeY;
        InitializeGame();
    }

    void InitializeGame()
    {
        foreach (var block in blockList.blocks)
        {
            GameObject blockObject = Instantiate(blockPrefab, gridContainer);
            blockObject.GetComponent<BlockController>().Setup(block.number);
        }
    }

    public void OnBlockSelected(BlockController selectedBlock)
    {
        if (status == GameStatus.Playing)
        {
            totalClicks++;
            if (firstSelected == null)
            {
                firstSelected = selectedBlock;
            }
            else
            {
                status = GameStatus.Waiting;
                if (firstSelected.Number == selectedBlock.Number)
                {
                    StartCoroutine(CorrectBlocks(firstSelected, selectedBlock));
                }
                else
                {
                    StartCoroutine(HideBlocks(firstSelected, selectedBlock));
                }
            }
          
        }
    }

    IEnumerator HideBlocks(BlockController first, BlockController second)
    {
       

        yield return new WaitForSeconds(0.5f);
        audioManager.IncorrectSound();
        first.HideBlock();
        second.HideBlock();

        yield return new WaitForSeconds(0.15f);
        firstSelected = null;
        status = GameStatus.Playing;
    }

    IEnumerator CorrectBlocks(BlockController first, BlockController second)
    {
       
        yield return new WaitForSeconds(0.5f);
        audioManager.CorrectSound();
        first.OnCorrectSelection();
        second.OnCorrectSelection();
        yield return new WaitForSeconds(0.15f);
        pairsFound++;
        UIManager.Instance.AddPair(pairsFound);
 
        if (pairsFound == totalPairs)
        {
            endTime = Time.time;
            OnFinishGame.Invoke();
        }
        firstSelected = null;
        status = GameStatus.Playing;
    }

    public void EndGame()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        float totalTime = endTime - startTime;
        int score = CalculateScore(totalClicks, totalTime);

        GameResult result = new GameResult
        {
            total_clicks = totalClicks,
            total_time = (int)totalTime,
            pairs = pairsFound,
            score = score
        };

        string json = JsonUtility.ToJson(result);
        string filePath = Path.Combine(Application.dataPath, "Resources/result.json");
        File.WriteAllText(filePath, json);

        leaderboardManager.AddOrUpdatePlayerScore(playerName, score);
        audioManager.EndMusic();
    }
    private int CalculateScore(int clicks, float time)
    {
        int score = 1000 - (clicks * 10) - (int)(time * 5);
        return score;
    }

    public void SetName(string name)
    {
        playerName = name;
    }

    public void RestartGame()
    {
        audioManager.MainMenuMusic();
    }
}

public enum GameStatus
{
    Playing,
    Waiting
}
