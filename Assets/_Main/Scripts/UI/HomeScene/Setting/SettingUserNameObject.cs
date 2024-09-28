

using MLib;
using TMPro;
using UnityEngine;

public class SettingUserNameObject: MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;

    public void ReadUserName()
    {
        inputName.text = DataManager.Instance.UserName;
    }
    public void UpdateUserName()
    {
        string input = inputName.text;
        if (IsValidInputName(input))
        {
            DataManager.Instance.UserName = input;
        }
        else
        {
            inputName.text = DataManager.Instance.UserName;
        }
    }

    private bool IsValidInputName(string input)
    {
        if (char.IsNumber(input[0])) return false;
        if (input.Length > 10) return false;

        return true;
    }
}