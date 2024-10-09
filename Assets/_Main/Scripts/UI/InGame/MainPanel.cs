using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using TMPro;
using DG.Tweening;

public class MainPanel : MPanel 
{
    [Header("Score")]
    [SerializeField] private TMP_Text tmpScoreValue;

    [Header("Rotate button")]
    [SerializeField] private GameObject clockwise_On;
    [SerializeField] private GameObject clockwise_Off;

    private void Start()
    {
        UpdateUIRotate();
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded()
    {
        ScoreSystem.Instance.OnScoreChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        ScoreSystem.Instance.OnScoreChanged -= OnScoreChanged;
    }

    public void OnClick_Pause()
    {
        //GamePlayController.Instance.PauseGame();
        MUIManager.Instance.ShowPanel<PausePanel>();
    }

    public void OnClick_Help(){
        MUIManager.Instance.ShowPanel<PanelHelp>();
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

    private void OnScoreChanged(int oldValue, int newValue)
    {
        DOVirtual.Int(oldValue, newValue, 0.5f, (value) =>
        {
            tmpScoreValue.text = value.ToString();
        });
    }
}
