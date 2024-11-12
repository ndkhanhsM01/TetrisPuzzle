using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using System;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.UI;

public class PanelHelp : MPanel
{
    [SerializeField] private ScrollRect scroll;
    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        GamePlayController.Instance.PauseGame();

        scroll.content.localPosition = Vector3.zero;
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
