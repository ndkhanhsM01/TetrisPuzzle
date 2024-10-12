using MLib;
using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class UserRanking_Respone
{
    [JsonProperty("ranking")] public int Ranking;
    [JsonProperty("top10")] public List<UserRanking_Respone> Top10;
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
        DataManager.OnLoadLocalSuccess += OnLoadLocalSuccess;
    }

    private void OnDisable()
    {
        EventsCenter.OnUserNameChanged -= UpdateNameGlobal;
        DataManager.OnLoadLocalSuccess -= OnLoadLocalSuccess;
    }

    private void OnLoadLocalSuccess(LocalData data)
    {
        bool isNewUser = data.userID < 0;
        if (!isNewUser) return;

        // create new global user data
        manager.HttpCaller.Post_CreateNewUser(data.userName, onSuccess: OnSuccess);

        void OnSuccess(User_Respone res)
        {
            DataManager.Instance.LocalData.userID = (int) res.SeqID;

            PanelRanking panelRanking = MUIManager.Instance.GetPanel<PanelRanking>();
            if (panelRanking)
            {
                panelRanking.Reload();
            }
        }
    }

    private void UpdateNameGlobal(string newName)
    {
        manager.HttpCaller.Post_UpdateName(DataManager.Instance.LocalData.userID, newName);
    }

    public override void Load()
    {
        
    }
}
