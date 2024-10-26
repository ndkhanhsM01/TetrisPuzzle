using MLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITouchSensitivitySetting : MonoBehaviour
{
    [SerializeField] private Slider slider;

    LocalData saveData;
    private void Awake()
    {
        slider.minValue = -1f;
        slider.maxValue = 1f;
    }

    private void OnEnable()
    {
        DataManager.OnLoadLocalSuccess += OnDataLoaded;
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
        slider.onValueChanged.AddListener(OnValueSliderChanged);
    }

    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= OnDataLoaded;
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        slider.onValueChanged.RemoveListener(OnValueSliderChanged);
    }

    private void OnDataLoaded(LocalData data)
    {
        saveData = data;
        slider.value = data.touchSensitivity;
    }

    private void OnSceneLoaded()
    {
        if (DataManager.Instance.LocalData == null) return;

        saveData = DataManager.Instance.LocalData;
        slider.value = saveData.touchSensitivity;
    }

    private void OnValueSliderChanged(float value)
    {
        if (saveData == null) return;
        saveData.touchSensitivity = value;
    }
}
