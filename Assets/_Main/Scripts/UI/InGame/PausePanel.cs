using MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MPanel
{
    public override void Show(Action onFinish)
    {
        base.Show(onFinish);

        GamePlayController.Instance.PauseGame();
    }

    public override void Hide(Action onFinish)
    {
        base.Hide(onFinish);

        GamePlayController.Instance.UnpauseGame();
    }
    public void OnClick_Restart()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.InGame);
    }
    public void OnClick_Resume()
    {
        this.Hide();
    } 
    public void OnClick_Home()
    {

    }
}
