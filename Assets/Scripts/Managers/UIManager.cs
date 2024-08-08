using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    [SerializeField] TextMeshProUGUI pairText;
    [SerializeField] GameObject startPanel;
    [Header("Start panel")]
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField nameInput;

    [Header("End panel")]
    [SerializeField] GameObject endPanel;
    [SerializeField] Button restartButton;

    int maxPairs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        nameInput.onValueChanged.AddListener(checkInput);

        startButton.onClick.AddListener(StartGame);
        startButton.onClick.AddListener(GameManager.Instance.OnStartGame.Invoke);
        GameManager.Instance.OnFinishGame.AddListener(FinishGame);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void Setup(int pairs)
    {
        maxPairs = pairs;
        pairText.text = "0 / " + maxPairs.ToString();
    }

    public void AddPair(int actualPairs)
    {
        pairText.text = actualPairs + " / " + maxPairs;
    }

    public void checkInput(string inputText)
    {
        if (inputText != "")
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);       
        GameManager.Instance.SetName(nameInput.text);
    }

    public void FinishGame()
    {

       endPanel.SetActive(true);
    }

    public void RestartGame()
    {
        endPanel.SetActive(false);
        startPanel.SetActive(true);
        GameManager.Instance.RestartGame();

    }
}
