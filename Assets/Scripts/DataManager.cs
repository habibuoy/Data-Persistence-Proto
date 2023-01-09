using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}
    public string PlayerName { get => playerName; set => playerName = value; }
    public HighScore HighScoreData { get => highScoreData; set => highScoreData = value; }

    private string playerName;
    private HighScore highScoreData;

    private const string HighScoreFileName = "/high_score.json";
    
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

        if (HighScoreData == null) HighScoreData = new HighScore("", 0);
    }

    private void Start() {
        
    }

    public void LoadScores()
    {

    }

    public void SaveHighScore()
    {
        // HighScore highScore = new HighScore(playerName, score);
        string json = JsonUtility.ToJson(HighScoreData);

        string path = Application.persistentDataPath + HighScoreFileName;
        File.WriteAllText(path, json);
        Debug.Log("Successfully saved high score");
    }

    public bool LoadHighScore()
    {
        string path = Application.persistentDataPath + HighScoreFileName;

        if (File.Exists(path))
        {
            string scoreFile = File.ReadAllText(path);
            
            HighScoreData = JsonUtility.FromJson<HighScore>(scoreFile);
            
            Debug.Log("Successfully loaded high score: " + HighScoreData.playerName + " - " + HighScoreData.score);

            return true;
        }

        return false;
    }

    public bool DeleteHighScore()
    {
        string path = Application.persistentDataPath + HighScoreFileName;

        if (File.Exists(path))
        {
            File.Delete(path);
            highScoreData.playerName = "";
            highScoreData.score = 0;
            
            Debug.Log("Successfully deleted high score");

            return true;
        }

        return false;
    }
    
    public HighScore GetHighScoreData()
    {
        return HighScoreData;
    }

    public class HighScore
    {
        
        public string playerName;
        public int score;

        public HighScore(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
}
