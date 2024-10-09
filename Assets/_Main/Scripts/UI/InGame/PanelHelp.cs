using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using System;

public class PanelHelp : MPanel
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
    public void OnClick_Close(){
        this.Hide();
    }
}
