using System;
using UnityEngine;

public class DifficutlyManager: MonoBehaviour
{
    [SerializeField] private GamePlayController gamePlay;

    [SerializeField] private int scoreStep = 1000;
    [SerializeField] private float incDropSpeedPercent = 0.05f;

    private void OnEnable()
    {
        ScoreSystem.OnScoreChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        ScoreSystem.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int preScore, int curScore)
    {
        int stepCount = curScore / scoreStep;
        int milestone = stepCount * scoreStep;

        if (preScore < milestone && curScore >= milestone)
        {
            float valueInc = stepCount * incDropSpeedPercent;

            gamePlay.IncFallingSpeed(valueInc);
            GamePlayController.OnLevelUp?.Invoke(stepCount);
            Debug.Log("Level up!!");
        }
    }
}