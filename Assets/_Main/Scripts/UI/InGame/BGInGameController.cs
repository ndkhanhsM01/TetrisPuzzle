using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLib;

public class BGInGameController : MonoBehaviour
{
    [SerializeField] private Image imgBG;

    private void OnEnable()
    {
        DataManager.OnLoadLocalSuccess += LoadBackground;
    }

    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= LoadBackground;
    }

    private void LoadBackground(LocalData localData)
    {
        SOItemBackground data = BackgroundInGameConfig.Instance.GetByID(localData.usingBackground);
        if(data == null)
        {
            imgBG.SetActive(false);
            return;
        }

        imgBG.SetActive(true);
        imgBG.sprite = data.Preview;
    }
}
