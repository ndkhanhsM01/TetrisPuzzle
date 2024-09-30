
using MLib;
using System;
using UnityEngine;

public class PanelRanking : MPanel
{
    [SerializeField] private FakeDataUsers fakeDataUsers;

    [Header("My score")]
    [SerializeField] private PlayerScoreItem myScoreItem;

    [Header("Top 3")]
    [SerializeField] private UserScoreItem[] scoreItems_Top3;

    [Header("Top 7")]
    [SerializeField] private UserScoreItem[] scoreItems_Top7;

    private UserRanking[] datasUser;

    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        FakeSetup();
    }

    public void FakeSetup()
    {
        Setup(fakeDataUsers.Datas);
    }

    public void Setup(UserRanking[] datasUser)
    {
        this.datasUser = datasUser;

        myScoreItem.Setup();
        SetupTop3(datasUser.GetRange(0, 3));
        SetupTop7(datasUser.GetRange(3, 10));
    }

    private void SetupTop3(UserRanking[] top3)
    {
        for(int i=0; i<top3.Length; i++)
        {
            scoreItems_Top3[i].Setup(top3[i]);
        }
    }

    private void SetupTop7(UserRanking[] top7)
    {
        for(int i=0; i< scoreItems_Top7.Length; i++)
        {
            var item = scoreItems_Top7[i];
            bool needShow = i < top7.Length;

            item.SetActive(needShow);

            if(needShow)
                item.Setup(top7[i]);
        }
    }

    private void SetupMyScore(UserRanking myScore)
    {

    }
}