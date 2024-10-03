using MLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "FakeDataUsers", menuName = "Tetris Setup/Fake Data Users")]
public class FakeDataUsers: ScriptableObject
{
    [SerializeField] private string[] randomNames;
    [SerializeField] private UserRanking[] datas;

    public UserRanking[] Datas => datas;
    void OnEnable()
    {
        // Ensure that changes made in Play Mode are not saved
        this.hideFlags = HideFlags.DontSave;
    }
    public void Set(int index, UserRanking data)
    {
        datas[index] = data;
    }

    public UserRanking Get(int index)
    {
        return datas[index];
    }

    public bool CheckInTop10(UserRanking userRanking)
    {
        UserRanking rankLowest = datas[datas.Length - 1];

        return userRanking.score >= rankLowest.score;
    }

#if UNITY_EDITOR
    [MButton]
    public void RandomDatas()
    {
        int amount = 10;
        datas = new UserRanking[amount];

        int scoreStart = 9600;
        int curTime = (int) TimeHelper.UnixTimeNow;
        int rangeTime = 604800;
        Vector2 stepRange = new Vector2(2000, 3000);
        Vector2 timeRange = new Vector2(curTime - rangeTime, curTime);

        int indexLast = datas.Length - 1;
        for(int i=0; i<amount; i++)
        {
            string randomName = randomNames.GetRandom();
            int index = indexLast - i;      // go to from small score to higher
            double time = Random.Range(timeRange.x, timeRange.y);

            int preIndex = Mathf.Clamp(index + 1, 0, indexLast);
            int score = scoreStart;
            if (datas[preIndex] != null) score = datas[preIndex].score;
            score += Random.Range((int)stepRange.x, (int)stepRange.y);

            UserRanking userRanking = new UserRanking()
            {
                id = (int) time,
                name = randomName,
                score = score,
                timestamp = time
            };

            datas[index] = userRanking;
        }
    }
#endif
}