

using MLib;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SettingUserNameObject: MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button btnSubmit;

    private void OnEnable()
    {
        btnSubmit.AddListener(OnClick_Submit);
        inputName.onEndEdit.AddListener(UpdateUserName);
    }

    private void OnDisable()
    {
        btnSubmit.RemoveListener(OnClick_Submit);
        inputName.onEndEdit.RemoveListener(UpdateUserName);
    }

    public void ReadUserName()
    {
        inputName.text = DataManager.Instance.UserName;
    }

    private void OnClick_Submit()
    {
        UpdateUserName(inputName.text);
    }

    private void UpdateUserName(string input)
    {
        //Debug.Log("Try update name: " + input);

        if (input == DataManager.Instance.UserName) return;

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