using MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingPanel : MPanel
{
    [SerializeField] private GameObject popup;
    [SerializeField] private SettingUserNameObject settingUserName;

    public override void Show(Action onFinish)
    {
        base.Show(onFinish);

        settingUserName.ReadUserName();
    }
}
