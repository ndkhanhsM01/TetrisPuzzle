using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLib;
using UnityEngine.UI;

public class CoinDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpCoinValue;
    [SerializeField] private Button btnCheat;

    private int curValue;

    private Tween tween;
    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
        DataManager.OnLoadLocalSuccess += OnLoadLocalSuccess;
        EventsCenter.OnCoinChanged += OnCoinChanged;

        LoadCurCoin();
#if ENABLE_CHEAT
        btnCheat.AddListener(OnClick_CheatCoin);
#endif
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        DataManager.OnLoadLocalSuccess -= OnLoadLocalSuccess;
        EventsCenter.OnCoinChanged -= OnCoinChanged;

#if ENABLE_CHEAT
        btnCheat.RemoveListener(OnClick_CheatCoin);
#endif
    }

    private void OnSceneLoaded()
    {
        LoadCurCoin();
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
    
    private void OnClick_CheatCoin()
    {
        DataManager.Instance.Coin += 1000;
    }

    private void LoadCurCoin()
    {
        if (!DataManager.Instance || DataManager.Instance.LocalData == null) return;

        OnCoinChanged(DataManager.Instance.Coin);
    }
}
