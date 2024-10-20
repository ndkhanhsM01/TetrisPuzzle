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
        DataManager.OnLoadLocalSuccess+= OnLoadLocalSuccess;
        EventsCenter.OnCoinChanged += OnCoinChanged;
    }

    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= OnLoadLocalSuccess;
        EventsCenter.OnCoinChanged -= OnCoinChanged;
    }

    private void OnLoadLocalSuccess(LocalData data)
    {
        OnCoinChanged(data.coin);
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
