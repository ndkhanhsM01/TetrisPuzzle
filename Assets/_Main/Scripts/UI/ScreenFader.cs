using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader: MSingleton<ScreenFader>
{
    [SerializeField] private Image imgCover;
    [SerializeField] private float durationFadeOutOnStart = 0.5f;
    public Image ImageCover => imgCover;

    private Tween tFade;
    private void Start()
    {
        FadeIn(0f, () =>
        {
            FadeOut(durationFadeOutOnStart);
        });
    }
    public void FadeIn(float duration, Action callback = null)
    {
        StopCurrentFade();
        imgCover.SetActive(true);
        tFade = imgCover.DOFade(1f, duration);
        tFade.OnComplete(() => callback?.Invoke());
    }
    public void FadeOut(float duration, Action callback = null)
    {
        StopCurrentFade();

        tFade = imgCover.DOFade(0f, duration);
        tFade.onComplete += () => callback?.Invoke();
        tFade.onComplete += () => imgCover.SetActive(false);
    }
    private void StopCurrentFade()
    {
        if (tFade != null) tFade.Kill();
    }
}
