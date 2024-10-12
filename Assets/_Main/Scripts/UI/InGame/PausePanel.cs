using MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MPanel
{
    private void OnApplicationFocus(bool focus)
    {
#if !UNITY_EDITOR
        if (!focus)
        {
            this.Show();
        }   
#endif
    }

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
        MSceneManager.Instance.LoadScene(ConstraintSceneName.Home);
    }
}
