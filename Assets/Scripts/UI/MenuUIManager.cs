using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIManager : MonoBehaviour
{
    public InputField nameInput;
    public Button playButton;
    public Button scoreBoardButton;
    public GameObject warningText;

    public GameObject scoreBoxPrefab;
    public GameObject scorePanel;

    // Start is called before the first frame update
    void Start()
    {
        nameInput.onValueChanged.AddListener(EvaluateNameInput);
        scoreBoardButton.onClick.AddListener(ScoresButton);

        playButton.interactable = false;
        warningText.SetActive(false);
    }
    
    public void EvaluateNameInput(string name)
    {
        if (name.Length > 3)
        {
            playButton.interactable = true;
            warningText.SetActive(false);
        }
        else
        {
            playButton.interactable = false;
            warningText.SetActive(true);
        }
    }

    public void LoadScores()
    {
        Transform content = scorePanel.transform.Find("ScoreView/Viewport/Content");
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        if (DataManager.Instance.LoadScores() && DataManager.Instance.PlayerScoreData.Count > 0)
        {
            List<DataManager.PlayerScore> tempScores = DataManager.Instance.PlayerScoreData;
            tempScores.Sort((a, b) => a.score.CompareTo(b.score));
            tempScores.Reverse();

            for (int i = 0; i < tempScores.Count; i++)
            {
                Text scoreText = Instantiate(scoreBoxPrefab, content).GetComponentInChildren<Text>();
                scoreText.text = (i + 1) + ". " + tempScores[i].name + " | " + tempScores[i].score;
            }

            Debug.Log("There are " + tempScores.Count + " scores");
        }
        else
        {
            Text scoreText = Instantiate(scoreBoxPrefab, content).GetComponentInChildren<Text>();
            scoreText.text = "No Scores Recorded";

            Debug.Log("There is no score");
        }
    }

    public void ScoresButton()
    {
        if (scorePanel.activeSelf)
        {
            scorePanel.SetActive(false);
            scoreBoardButton.GetComponentInChildren<Text>().text = "Show Score Board";
        }
        else
        {
            LoadScores();
            scorePanel.SetActive(true);
            scoreBoardButton.GetComponentInChildren<Text>().text = "Close";
        }
    }

    public void PlayButton()
    {
        DataManager.Instance.PlayerName = nameInput.text;
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

        #else
        Application.Quit();

        #endif
    }

    public void DeleteHighScore()
    {
        if (DataManager.Instance.DeleteScores())
        {
            scoreBoardButton.interactable = false;
        }
    }
}
