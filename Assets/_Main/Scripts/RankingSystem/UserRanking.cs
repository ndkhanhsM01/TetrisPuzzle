

using System;

[System.Serializable]
public class UserRanking
{
    public int id;
    public int index;
    public string name;
    public double timestamp;
    public int score;

    public static UserRanking CreateNew(string name, int score, double timestamp = -1)
    {
        double timeData = timestamp;
        if(timeData == -1)
        {
            timeData = TimeHelper.UnixTimeNow;
        }

        UserRanking newData = new UserRanking()
        {
            id = (int) timestamp,
            name = name,
            score = score,
            timestamp = timeData
        };
        
        return newData;
    }

    public string GetTimeFormat_1()
    {
        if (timestamp <= 0) return null;

        DateTime dateTime = TimeHelper.UnixTimeStampToDateTime(timestamp);
        return dateTime.ToString("yyyy/MM/dd HH:mm");
    }
}