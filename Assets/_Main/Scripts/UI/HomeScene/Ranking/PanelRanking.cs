
using MLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class PanelRanking : MPanel
{
    [SerializeField] private FakeDataUsers fakeDataUsers;

    [Header("My score")]
    [SerializeField] private PlayerScoreItem myScoreItem;

    [Header("Top 3")]
    [SerializeField] private UserScoreItem[] scoreItems_Top3;

    [Header("Top 7")]
    [SerializeField] private UserScoreItem[] scoreItems_Top7;

    private int indexPlayer;
    private UserRanking playerRanking;
    private List<UserRanking> datasUser;

    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += SetupData;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= SetupData;
        
    }

    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        FakeSetup();
    }

    public void FakeSetup()
    {
        Setup(datasUser);
    }

    public void Setup(List<UserRanking> datasUser)
    {
        this.datasUser = datasUser;

        myScoreItem.Setup(indexPlayer, playerRanking);

        SetupTop3(datasUser.GetRange(0, 3));
        SetupTop7(datasUser.GetRange(3, 7));
    }

    private void SetupTop3(List<UserRanking> top3)
    {
        for(int i=0; i<top3.Count; i++)
        {
            scoreItems_Top3[i].Setup(i, top3[i]);
        }
    }

    private void SetupTop7(List<UserRanking> top7)
    {
        for(int i=0; i< scoreItems_Top7.Length; i++)
        {
            var item = scoreItems_Top7[i];
            bool needShow = i < top7.Count;

            item.SetActive(needShow);

            if(needShow)
                item.Setup(3 + i, top7[i]);
        }
    }

    private void SetupData()
    {
        var localData = DataManager.Instance.LocalData;
        playerRanking = UserRanking.CreateNew(localData.userName, localData.highScore, localData.userID);

        datasUser = new();
        datasUser.AddRange(fakeDataUsers.Datas);
        datasUser.Add(playerRanking);

        if (fakeDataUsers.CheckInTop10(playerRanking))
        {
            datasUser = datasUser.OrderByDescending(user => user.score).ToList();

            indexPlayer = datasUser.IndexOf(playerRanking);
        }
        else
        {
            indexPlayer = Random.Range(20, 40);
        }
    }
}