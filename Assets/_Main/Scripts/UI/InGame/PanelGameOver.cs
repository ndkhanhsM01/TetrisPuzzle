using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using TMPro;
public class PanelGameOver : MPanel
{
    [SerializeField] private TMP_Text tmpScore;
    [SerializeField] private TMP_Text tmpCoin;

    [SerializeField] private GameObject goNewHighScore;

    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
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
        Setup();
    }

    public void Setup()
    {
        int score = ScoreSystem.Instance.CurrentScore;
        int coin = ScoreSystem.Instance.CurCoin;

        bool isHighScore = DataManager.Instance.TrySetNewHighScore(score);
        goNewHighScore.SetActive(isHighScore);

        tmpScore.text = score.ToString();
        tmpCoin.text = coin.ToString();

        DataManager.Instance.Coin += coin;
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
