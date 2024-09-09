using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
public class PanelGameOver : MPanel
{
    public void OnClick_Retry()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.InGame);
    }

    public void OnClick_Close()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.Home, 1f);
    }
}
