using Cysharp.Threading.Tasks;
using MLib;
using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class UserRanking_Respone
{
    [JsonProperty("ranking")] public int Ranking;
    [JsonProperty("top10")] public List<UserRanking> Top10;
}

[System.Serializable]
public class User_Respone
{
    [JsonProperty("seqID")] public long SeqID;
    [JsonProperty("name")] public string Name;
    [JsonProperty("score")] public string Score;
}

public class UserScoreGlobal : GlobalData<User_Respone>
{
    private void OnEnable()
    {
        EventsCenter.OnUserNameChanged += UpdateNameGlobal;
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
        DataManager.OnLoadLocalSuccess += OnLoadLocalSuccess;
    }

    private void OnDisable()
    {
        EventsCenter.OnUserNameChanged -= UpdateNameGlobal;
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        DataManager.OnLoadLocalSuccess -= OnLoadLocalSuccess;
    }

    private async void OnLoadLocalSuccess(LocalData data)
    {
        await UniTask.WaitUntil(() => GlobalDataManager.Instance);

        UpdateData(data);
    }

    private void UpdateNameGlobal(string newName)
    {
        manager.HttpCaller.Post_UpdateName(DataManager.Instance.LocalData.userID, newName);
    }
    private void OnSceneLoaded()
    {
        if (DataManager.Instance == null || DataManager.Instance.IsLoadSuccess == false) return;

        UpdateData(DataManager.Instance.LocalData);
    }
    public override void Load()
    {
        
    }

    public void UpdateData(LocalData data)
    {
        manager.HttpCaller.Post_UpdateScore(data.userID, data.highScore);
        manager.HttpCaller.Post_UpdateName(data.userID, data.userName);
    }
}
