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
        EventsCenter.OnSceneLoaded += LoadBackground;
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= LoadBackground;
    }

    private void LoadBackground()
    {
        if (!DataManager.Instance) return;
        SOItemBackground data = BackgroundInGameConfig.Instance.GetByID(DataManager.Instance.LocalData.usingBackground);
        if(data == null)
        {
            imgBG.SetActive(false);
            return;
        }

        imgBG.SetActive(true);
        imgBG.sprite = data.Preview;
    }
}
