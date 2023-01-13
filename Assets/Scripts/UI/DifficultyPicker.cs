using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPicker : MonoBehaviour
{
    public DataManager.Difficulty[] difficulties;
    public Text difficultyText;
    public Dropdown difficultyDropdown;

    private DataManager.Difficulty chosenDifficulty;
    
    public event System.Action<int> onValueChanged;

    public void Init(MenuUIManager menuUIManager)
    {
        difficultyDropdown = GetComponent<Dropdown>();
        difficultyDropdown.onValueChanged.AddListener(SelectDifficulty);
        onValueChanged += menuUIManager.SetDifficulty;

        int difficultyValue = 1;
        if (DataManager.Instance.GetDifficulty.value > 0)
        {
            // SelectDifficulty(DataManager.Instance.GetDifficulty.value);
            difficultyValue = DataManager.Instance.GetDifficulty.value;
        }

        SelectDifficulty(difficultyValue);
        difficultyDropdown.value = difficultyValue;
    }

    public void SelectDifficulty(int value)
    {
        onValueChanged.Invoke(value);

        if (value == 0) 
        {
            ChangeDifficultyText("Choose Difficulty");
            return;
        }

        chosenDifficulty = difficulties[value - 1];
        ChangeDifficultyText(chosenDifficulty.description);
        DataManager.Instance.SelectDifficulty(chosenDifficulty);
    }

    public void ChangeDifficultyText(string text)
    {
        difficultyText.text = text;
        // difficultyText.color = chosenDifficulty.textColor;
    }
}
