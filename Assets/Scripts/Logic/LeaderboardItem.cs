using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        
    }
    public void SetLeaderboardItem(string username, int score)
    {
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}
