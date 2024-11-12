using MLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "FakeDataUsers", menuName = "Tetris Setup/Fake Data Users")]
public class FakeDataUsers: ScriptableObject
{
    [SerializeField] private string[] randomNames;

    public bool CheckInTop10(UserInfo userRanking)
    {
        return false;
/*        if (RankManager.Instance == null) return false;

        var datas = RankManager.Instance.Data.AllUsers;
        UserInfo rankLowest = datas[datas.Count - 1];

        return userRanking.score >= rankLowest.score;*/
    }

    public async UniTask<List<UserInfo>> SpawnNewFakeList(string namePlayer, int scorePlayer, int amount)
    {
        List<UserInfo> datas = new();

        UserInfo playerInfo = new UserInfo()
        {
            id = 0,
            name = namePlayer,
            score = scorePlayer
        };
        datas.Add(playerInfo);

        //
        int scoreStart = 9600;
        int preScore = 0;
        Vector2 stepRange = new Vector2(2000, 3000);

        for(int i=amount; i>0; i--)
        {
            await UniTask.DelayFrame(1);
            string randomName = randomNames.GetRandom();

            int score = scoreStart;
            score += preScore * i + Random.Range((int)stepRange.x, (int)stepRange.y);

            UserInfo userRanking = new UserInfo()
            {
                id = i,
                name = randomName,
                score = score
            };

            preScore = score;
            datas.Add(userRanking);
        }

        return datas;
    }
}