using MLib;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIToggleAudio: MonoBehaviour
{
    [SerializeField] private AudioType type;
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprOn;
    [SerializeField] private Sprite sprOff;

    private bool isActive;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (!DataManager.Instance) return;

        switch (type)
        {
            case AudioType.Music: isActive = DataManager.Instance.LocalData.isPlayMusic; break;
            case AudioType.SoundFX: isActive = DataManager.Instance.LocalData.isPlaySoundFX; break;
        }

        UpdateUI();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(Click_Toggle);   
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Click_Toggle);
    }

    private void Click_Toggle()
    {
        if (!DataManager.Instance) return;

        isActive = !isActive;

        switch (type)
        {
            case AudioType.Music: DataManager.Instance.LocalData.isPlayMusic = isActive; break;
            case AudioType.SoundFX: DataManager.Instance.LocalData.isPlaySoundFX = isActive; break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        image.sprite = isActive ? sprOn : sprOff;
    }

    public enum AudioType
    {
        Music, SoundFX
    }
}

