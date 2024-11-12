using DG.Tweening;
using UnityEngine;

public class UIScaleOnEnable: MonoBehaviour
{
    [SerializeField] private float delay = 0f;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease ease = Ease.OutBack;
    [SerializeField] private Vector3 startValue = Vector3.zero;
    [SerializeField] private Vector3 endValue = Vector3.one;

    private Tween tweenScale;
    private void OnEnable()
    {
        DoScale();
    }

    private void OnDisable()
    {
        Kill();
    }

    private void DoScale()
    {
        Kill();

        transform.localScale = startValue;

        tweenScale = transform.DOScale(endValue, duration);
        tweenScale.SetDelay(delay);
        tweenScale.SetEase(ease);
    }

    private void Kill()
    {
        if (tweenScale != null) tweenScale.Kill();
    }
}
