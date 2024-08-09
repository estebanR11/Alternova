using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries;
}
public class LeaderboardManager : MonoBehaviour
{
    private List<LeaderboardEntry> leaderboardEntries;
    [SerializeField] Leaderboard leaderboard;

    private void OnEnable()
    {
        leaderboardEntries = new List<LeaderboardEntry>();

    }

    public void AddOrUpdatePlayerScore(string username, int score)
    {
        LeaderboardEntry entry = leaderboardEntries.Find(e => e.username == username);
        if (entry != null)
        {
            entry.score = score;
        }
        else
        {
            leaderboardEntries.Add(new LeaderboardEntry { username = username, score = score });
        }
        SaveLeaderboardData();
    }

    public void DisplayLeaderboard()
    {
        leaderboard.DestroyItems();
        foreach (var entry in leaderboardEntries)
        {
            leaderboard.DisplayLeaderboard(entry.username, entry.score);
        }
    }

    public void LoadLeaderboardData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/leaderboard.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);

            leaderboardEntries = data.entries ?? new List<LeaderboardEntry>();
            leaderboardEntries.Sort((a, b) => b.score.CompareTo(a.score));
        }
        DisplayLeaderboard();
    }

    private void SaveLeaderboardData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/leaderboard.json");
        LeaderboardData data = new LeaderboardData
        {
            entries = leaderboardEntries
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
        LoadLeaderboardData();
    }
}
