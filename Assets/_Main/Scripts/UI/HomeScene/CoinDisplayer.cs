using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLib;

public class CoinDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpCoinValue;

    private int curValue;

    private Tween tween;
    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
        EventsCenter.OnCoinChanged += OnCoinChanged;
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        EventsCenter.OnCoinChanged -= OnCoinChanged;
    }

    private void OnSceneLoaded()
    {
        OnCoinChanged(DataManager.Instance.Coin);
    }

    private void OnCoinChanged(int newValue)
    {
        if (gameObject.activeInHierarchy)
        {
            if (tween != null) tween.Kill();
            tween = DOVirtual.Int(curValue, newValue, 0.35f, (value) =>
            {
                tmpCoinValue.text = $"{value}";
            });
        }

        curValue = newValue;
    }
}
