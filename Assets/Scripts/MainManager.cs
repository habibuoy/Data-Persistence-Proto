using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount = 6;
    public Rigidbody ball;

    public Text scoreText;
    public Text bestScoreText;
    public GameObject gameOverPanel;
    
    private bool isStarted = false;
    private int score;
    
    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.pointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        DataManager.PlayerScore best = DataManager.Instance.GetBestScore();

        UpdateBestScore(best.name, best.score);
    }

    private void Update()
    {
        if (!isStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f * DataManager.Instance.GetDifficulty.value, ForceMode.VelocityChange);
            }
        }
        else if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        score += point * DataManager.Instance.GetDifficulty.value;
        scoreText.text = $"Score : {score}";

        if (IsCurrentPointHighest())
        {
            UpdateBestScore(DataManager.Instance.PlayerName, score);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);

        DataManager.Instance.SaveScore(new DataManager.PlayerScore(){name = DataManager.Instance.PlayerName, score = score, difficulty = DataManager.Instance.GetDifficulty.name});
    }

    private bool IsCurrentPointHighest()
    {
        return score > DataManager.Instance.BestScore.score;
    }

    private void UpdateBestScore(string name, int score)
    {
        string text = "Best Score: " +  name + " | " + score + " | " + DataManager.Instance.GetDifficulty.name;

        if (string.IsNullOrEmpty(name) && score == 0)
        {
            text = "No Best Score Yet";
        }

        bestScoreText.text = text;
    }
}
