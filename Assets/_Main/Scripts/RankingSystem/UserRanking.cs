

using MLib;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class UserRanking
{
    [JsonProperty("seqID")]     public int id;
    //public int index;
    [JsonProperty("name")]      public string name;
    [JsonProperty("timeStamp")] public double timestamp;
    [JsonProperty("score")]     public int score;

    public static UserRanking CreateNew(string name, int score, int id, double timestamp = -1)
    {
        double timeData = timestamp;
        if(timeData == -1)
        {
            timeData = TimeHelper.UnixTimeNow;
        }

        UserRanking newData = new UserRanking()
        {
            id = id,
            name = name,
            score = score,
            timestamp = timeData
        };
        
        return newData;
    }

    public bool IsUser
    {
        get
        {
            if (DataManager.Instance)
                return id == DataManager.Instance.LocalData.userID;

            return false;
        }
    }

    public string GetTimeFormat_1()
    {
        if (timestamp <= 0) return null;

        DateTime dateTime = TimeHelper.UnixTimeStampToDateTime(timestamp);
        return dateTime.ToString("yyyy/MM/dd HH:mm");
    }
}