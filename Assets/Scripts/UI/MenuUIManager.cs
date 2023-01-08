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
    public GameObject warningText;

    // Start is called before the first frame update
    void Start()
    {
        nameInput.onValueChanged.AddListener(EvaluateNameInput);

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

    public void PlayButton()
    {
        DataManager.Instance.playerName = nameInput.text;
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
}
