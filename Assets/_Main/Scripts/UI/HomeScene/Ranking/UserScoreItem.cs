

using TMPro;
using UnityEngine;

public class UserScoreItem: MonoBehaviour
{
    [SerializeField] private TMP_Text tmpIndex;
    [SerializeField] private TMP_Text tmpName;
    [SerializeField] private TMP_Text tmpTime;
    [SerializeField] private TMP_Text tmpScore;

    public void Setup(int index, UserRanking userRanking)
    {
        if(tmpIndex) tmpIndex.text = (index + 1).ToString();
        if(tmpName) tmpName.text = userRanking.name;
        if(tmpTime) tmpTime.text = $"#{userRanking.id}";
        if(tmpScore) tmpScore.text = userRanking.score.ToString();
    }
}