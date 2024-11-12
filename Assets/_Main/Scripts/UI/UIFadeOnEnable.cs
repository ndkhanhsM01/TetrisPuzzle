using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeOnEnable : MonoBehaviour
{
    [SerializeField] private float delay = 0f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private CanvasGroup group;

    private Tween tweenFade;

    private void Reset()
    {
        group = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        DoFade();
    }

    private void OnDisable()
    {
        Kill();
    }

    private void DoFade()
    {
        Kill();

        if(!group) group = GetComponent<CanvasGroup>();

        group.alpha = startValue;

        tweenFade = group.DOFade(endValue, duration);
        tweenFade.SetDelay(delay);
        tweenFade.SetEase(ease);
    }

    private void Kill()
    {
        if (tweenFade != null) tweenFade.Kill();
    }
}
