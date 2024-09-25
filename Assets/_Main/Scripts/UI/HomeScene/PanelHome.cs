using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
public class PanelHome : MPanel
{
    public void OnClick_Play()
    {
        MSceneManager.Instance.LoadScene(ConstraintSceneName.InGame);
    }

    public void OnClick_Rank()
    {

    }
    public void OnClick_Shop()
    {
        MUIManager.Instance.ShowPanel<PanelShop>();
    }
    public void OnClick_Setting()
    {

    }
}
