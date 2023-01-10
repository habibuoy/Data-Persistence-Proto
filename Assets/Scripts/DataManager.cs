using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}
    public string PlayerName { get => playerName; set => playerName = value; }
    public List<PlayerScore> PlayerScoreData { get => playerScoreData; set => playerScoreData = value; }
    public PlayerScore BestScore { get => bestScore; set => bestScore = value; }

    private string playerName;
    private List<PlayerScore> playerScoreData;
    private PlayerScore bestScore;

    private const string ScoresFileName = "/player_scores.json";

    private string scoresPath;
    
    private void Awake() {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerScoreData == null) playerScoreData = new List<PlayerScore>();

        scoresPath = Application.persistentDataPath + ScoresFileName;
        LoadScores();

        BestScore = GetBestScore();
    }

    private void Start() {
        
    }

    public bool LoadScores()
    {
        if (File.Exists(scoresPath))
        {
            string scoreFile = File.ReadAllText(scoresPath);

            Scores scoreData = JsonUtility.FromJson<Scores>(scoreFile);

            playerScoreData.Clear();
            
            foreach (PlayerScore score in scoreData.data) // remove duplicate data
            {
                if (!playerScoreData.Contains(score))
                {
                    playerScoreData.Add(score);
                }
            }

            // Debug.Log("Successfully loaded scores");

            return true;
        }

        return false;
    }

    public void SaveScore(PlayerScore playerScore)
    {
        if (playerScoreData.Contains(playerScore))
        {
            int index = playerScoreData.IndexOf(playerScore);
            int oldScore = playerScoreData[index].score;
            int newScore = playerScore.score;
            playerScoreData[index].score = oldScore > newScore ? oldScore : newScore; // save if new score is higher than the old score
        }
        else
        {
            playerScoreData.Add(playerScore);
        }

        Scores scoreData = new Scores();
        scoreData.data = playerScoreData;
        string json = JsonUtility.ToJson(scoreData, true);
        File.WriteAllText(scoresPath, json);

        // Debug.Log("Successfully saved scores");
    }

    public bool DeleteScores()
    {
        if (File.Exists(scoresPath))
        {
            File.Delete(scoresPath);
            playerScoreData.Clear();
            
            Debug.Log("Successfully deleted all scores");

            return true;
        }

        return false;
    }
    
    public PlayerScore GetBestScore()
    {
        if (playerScoreData == null || playerScoreData.Count == 0) return null;

        // int highestScore = int.MinValue;
        PlayerScore playerHighestScore = new PlayerScore();

        foreach (PlayerScore ps in playerScoreData)
        {
            if (ps.score > playerHighestScore.score)
            {
                playerHighestScore = ps;
            }
        }

        return playerHighestScore;
    }

    [System.Serializable]
    public class Scores
    {
        public List<PlayerScore> data;
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string name;
        public int score;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PlayerScore o = obj as PlayerScore;

            if (o == null)
            {
                return false;
            }

            return o.name.Equals(this.name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, score);
        }
    }
}
