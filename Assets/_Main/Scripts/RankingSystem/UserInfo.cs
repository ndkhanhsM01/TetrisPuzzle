

using MLib;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class UserInfo
{
    [JsonProperty("seqID")]     public int id;
    //public int index;
    [JsonProperty("name")]      public string name;
    [JsonProperty("score")]     public int score;

    public static UserInfo CreateNew(string name, int score, int id)
    {
        UserInfo newData = new UserInfo()
        {
            id = id,
            name = name,
            score = score
        };
        
        return newData;
    }

    public bool IsMe
    {
        get
        {
            if (DataManager.Instance)
                return id == DataManager.Instance.LocalData.userID;

            return false;
        }
    }
}