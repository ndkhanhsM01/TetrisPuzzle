using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using System;
using UnityEngine.UIElements;
using DG.Tweening;

public class PanelHelp : MPanel
{
    [SerializeField] private RectTransform content;
    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        GamePlayController.Instance.PauseGame();

        DOVirtual.DelayedCall(0.05f, () =>
        {
            content.localPosition = Vector3.zero;
        });
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
