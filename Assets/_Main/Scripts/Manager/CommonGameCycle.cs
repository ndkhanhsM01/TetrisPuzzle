using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
public class CommonGameCycle : MSingleton<CommonGameCycle>
{
    protected override void Awake()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 60;
    }
}
