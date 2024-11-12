using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using TMPro;
using System;
public class PanelGameOver : MPanel
{
    [SerializeField] private TMP_Text tmpScore;
    [SerializeField] private TMP_Text tmpCoin;

    [SerializeField] private GameObject goContent;
    [SerializeField] private GameObject goNewHighScore;

    private void Start()
    {
        goContent.SetActive(false);
    }

    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        GamePlayController.Instance.OnGameEnd -= OnEndGame;
    }

    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        goContent.SetActive(true);
        MAudioManager.Instance.SetVolumeMusic(0f);
    }

    private void ReturnBGMusic()
    {
        if (DataManager.Instance.LocalData.isPlayMusic) MAudioManager.Instance.SetVolumeMusic(1f);
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

        var audioManager = MAudioManager.Instance;
        if(audioManager != null)
        {
            if (isHighScore) audioManager.PlaySFX(MSoundType.Win);
            else audioManager.PlaySFX(MSoundType.GameOver);
        }
    }

    public void OnClick_Retry()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.InGame);
        ReturnBGMusic();
    }

    public void OnClick_Close()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.Home, 1f);
        ReturnBGMusic();
    }
}
