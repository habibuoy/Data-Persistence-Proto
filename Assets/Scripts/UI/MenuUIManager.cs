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

    public Color[] colors;
    public GameObject colorButtons;
    private List<Button> buttons;
    public GameObject buttonPrefab;

    public DifficultyPicker difficultyPicker;

    private int difficultyValue;
    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        difficultyPicker.Init(this);
        playButton.interactable = false;
        warningText.SetActive(false);

        // buttons = new List<Button>();
        // SetupColorButtons();
    }

    private void OnEnable() {
        nameInput.onValueChanged.AddListener(EvaluateNameInput);
        scoreBoardButton.onClick.AddListener(ScoresButton);
    }

    private void OnDisable() {
        nameInput.onValueChanged.RemoveListener(EvaluateNameInput);
        scoreBoardButton.onClick.RemoveListener(ScoresButton);
    }

    public void SetDifficulty(int value)
    {
        difficultyValue = value;
        HandlePlayButton();
    }
    
    public void EvaluateNameInput(string name)
    {
        if (name.Length > 3)
        {
            // playButton.interactable = true;
            warningText.SetActive(false);
        }
        else
        {
            // playButton.interactable = false;
            warningText.SetActive(true);
        }
        HandlePlayButton();
    }

    public void HandlePlayButton()
    {
        if (name.Length < 3 || difficultyValue == 0)
        {
            playButton.interactable = false;
        }
        else if (name.Length > 3 && difficultyValue > 0)
        {
            playButton.interactable = true;
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
                scoreText.text = (i + 1) + ". " + tempScores[i].name + " | " + tempScores[i].score + " | " + tempScores[i].difficulty;
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
        DataManager.Instance.SaveDifficulty();
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

    // private void SetupColorButtons()
    // {
    //     foreach (Color c in colors)
    //     {
    //         Button b = Instantiate(buttonPrefab, colorButtons.transform).GetComponent<Button>();
    //         b.GetComponent<Image>().color = c;
    //         b.onClick.AddListener(() => {
    //             SetColor(c);
    //             foreach (Button b in buttons)
    //             {
    //                 b.interactable = true;
    //             }
    //             b.interactable = false;
    //         });

    //         buttons.Add(b);
    //     }

    //     // for (int i = 0; i < colors.Length; i++)
    //     // {
    //     //     buttons[i].GetComponent<Image>().color = colors[i];
    //     // }
    // }

    // public void SetColor(Color c)
    // {
    //     DataManager.Instance.chosenColor = c;
    // }
}
