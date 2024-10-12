
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

    private bool isLoaded = false;
    private void OnEnable()
    {
        DataManager.OnLoadLocalSuccess += SetupData;
    }
    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= SetupData;
        
    }

    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        Reload();
    }

    public void Reload()
    {
        if (!isLoaded)
        {
            foreach (UserScoreItem item in scoreItems_Top3) item.SetActive(false);
            foreach (UserScoreItem item in scoreItems_Top7) item.SetActive(false);
            myScoreItem.SetActive(false);
        }

        SetupData(DataManager.Instance.LocalData);
    }

    public void SetupData(LocalData localData)
    {
        GlobalDataManager.Instance.HttpCaller.Get_GetRankingResult(localData.userID, onSuccess: OnLoadDataSuccess);
    }

    private void OnLoadDataSuccess(UserRanking_Respone res)
    {
        var localData = DataManager.Instance.LocalData;
        playerRanking = UserRanking.CreateNew(localData.userName, localData.highScore, localData.userID);
        indexPlayer = res.Ranking;

        datasUser = new();
        //datasUser.Add(playerRanking);
        datasUser.AddRange(res.Top10);
        datasUser = datasUser.OrderByDescending(user => user.score).ToList();

        isLoaded = true;
        UpdateUI();
    }

    private void SetUpFakeData(UserRanking playerRanking)
    {
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

    #region Set up UI
    private void UpdateUI()
    {
        myScoreItem.Setup(indexPlayer, playerRanking);
        myScoreItem.SetActive(true);

        if(datasUser.Count > 0)
        {
            List<UserRanking> list = new List<UserRanking>();
            for (int i = 0; i < 3; i++)
            {
                list.Add(datasUser[i]);
            }
            Debug.Log(list.Count);
            SetupTop3(list);
        }

        if(datasUser.Count > 3)
        {
            List<UserRanking> list = new List<UserRanking>();
            for(int i=3; i<datasUser.Count; i++)
            {
                list.Add(datasUser[i]);
            }
            SetupTop7(list);
        }
    }

    private void SetupTop3(List<UserRanking> top3)
    {
        for (int i = 0; i < scoreItems_Top3.Length; i++)
        {
            var item = scoreItems_Top3[i];
            bool needShow = i < top3.Count;
            item.SetActive(needShow);
            if (needShow)
            {
                item.Setup(i, top3[i]);
            }
        }
    }

    private void SetupTop7(List<UserRanking> top7)
    {
        for (int i = 0; i < scoreItems_Top7.Length; i++)
        {
            var item = scoreItems_Top7[i];
            bool needShow = i < top7.Count;

            item.SetActive(needShow);

            if (needShow)
                item.Setup(3 + i, top7[i]);
        }
    }
    #endregion
}