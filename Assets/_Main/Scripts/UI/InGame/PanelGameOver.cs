using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
public class PanelGameOver : MPanel
{
    private void OnEnable()
    {
        EventsCenterInGame.OnGameEnd += OnEndGame;
    }

    private void OnDisable()
    {
        EventsCenterInGame.OnGameEnd -= OnEndGame;
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
