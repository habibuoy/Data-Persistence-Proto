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
    public Button highScoreButton;
    public GameObject warningText;

    // Start is called before the first frame update
    void Start()
    {
        nameInput.onValueChanged.AddListener(EvaluateNameInput);
        highScoreButton.onClick.AddListener(DeleteHighScore);

        playButton.interactable = false;
        warningText.SetActive(false);

        highScoreButton.interactable = false;

        if (DataManager.Instance.LoadHighScore())
        {
            highScoreButton.interactable = true;
        }
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
        if (DataManager.Instance.DeleteHighScore())
        {
            highScoreButton.interactable = false;
        }
    }
}
