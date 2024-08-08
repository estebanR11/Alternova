using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private Transform leaderboardContainer;


    public void DisplayLeaderboard(string name, int score)
    {
        GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardContainer);
        leaderboardItem.GetComponent<LeaderboardItem>().SetLeaderboardItem(name, score);
              
    }

    public void DestroyItems()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
