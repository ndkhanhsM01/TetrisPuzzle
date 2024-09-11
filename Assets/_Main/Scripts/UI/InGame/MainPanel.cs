using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;

public class MainPanel : MPanel 
{
    [Header("Rotate button")]
    [SerializeField] private GameObject clockwise_On;
    [SerializeField] private GameObject clockwise_Off;

    private void Start()
    {
        UpdateUIRotate();
    }
    public void OnClick_Pause()
    {
        GamePlayController.Instance.PauseGame();
        MUIManager.Instance.ShowPanel<PausePanel>();
    }

    public void OnClick_SwitchRotate()
    {
        bool newValue = !GamePlayController.Instance.IsRotateClockwise;
        GamePlayController.Instance.IsRotateClockwise = newValue;
        UpdateUIRotate();
    }

    private void UpdateUIRotate()
    {
        if (!GamePlayController.Instance) return;
        if (GamePlayController.Instance.IsRotateClockwise)
        {
            clockwise_On.SetActive(true);
            clockwise_Off.SetActive(false);
        }
        else
        {
            clockwise_On.SetActive(false);
            clockwise_Off.SetActive(true);
        }
    }
}
