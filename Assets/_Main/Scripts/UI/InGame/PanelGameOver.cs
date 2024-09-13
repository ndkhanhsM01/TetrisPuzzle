using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
public class PanelGameOver : MPanel
{
    private void Start()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GamePlayController.Instance.OnGameEnd -= OnEndGame;
    }
    private void OnSceneLoaded()
    {
        GamePlayController.Instance.OnGameEnd += OnEndGame;
    }
    private void OnEndGame(EndGameStatus status)
    {
        if (status == EndGameStatus.Win) return;

        this.Show();
    }

    public void OnClick_Retry()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.InGame);
    }

    public void OnClick_Close()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.Home, 1f);
    }
}
