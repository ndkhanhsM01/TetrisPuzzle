using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;

public class MainPanel : MPanel 
{
    public void OnClick_Pause()
    {
        GamePlayController.Instance.OnPauseGame?.Invoke();
        MUIManager.Instance.ShowPanel<PausePanel>();
    }
}
