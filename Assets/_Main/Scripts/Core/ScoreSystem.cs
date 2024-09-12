

using MLib;
using System;
using UnityEngine;

public class ScoreSystem: MSingleton<ScoreSystem>
{
    [SerializeField] private int score = 0;
    [SerializeField] private int scorePerRow = 100;
    [SerializeField] private int multiplePerCombo = 1;
    private int comboCount = 0;
    private bool inCombo = false;

    public Action<int, int> OnScoreChanged;

    public bool InCombo => inCombo;
    public int CurrentScore => score;

    private void Start()
    {
        OnGameReady();
    }
    private void OnEnable()
    {
        //MSceneManager.Instance.OnSceneReady += OnGameReady;
    }
    private void OnDisable()
    {
        //MSceneManager.Instance.OnSceneReady -= OnGameReady;
    }

    private void OnGameReady()
    {
        score = 0;
        OnScoreChanged?.Invoke(0, 0);
    }

    public void StartNewCombo()
    {
        comboCount = 0;
        inCombo = true;
    }

    public void IncreaseComboCount()
    {
        comboCount++;
    }
    public void UpdateScoreInCombo()
    {
        if (!inCombo || comboCount <= 0) return;

        int newScore = score;
        for (int i = 1; i <= comboCount; i++)
        {
            int rewardScore = i * scorePerRow * multiplePerCombo;
            newScore += rewardScore;
        }

        OnScoreChanged?.Invoke(score, newScore);
        score = newScore;
    }

    public void EndCurrentCombo()
    {
        inCombo = false;   
        comboCount = 0;
    }
}