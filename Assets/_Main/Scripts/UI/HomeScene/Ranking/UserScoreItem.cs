

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreItem: MonoBehaviour
{
    [SerializeField] private TMP_Text tmpIndex;
    [SerializeField] private TMP_Text tmpName;
    [SerializeField] private TMP_Text tmpTime;
    [SerializeField] private TMP_Text tmpScore;
    [SerializeField] private GameObject goHighlight;

    public void Setup(int index, UserInfo userRanking)
    {
        if(goHighlight) goHighlight.SetActive(userRanking.IsMe);
        if(tmpIndex) tmpIndex.text = (index + 1).ToString();
        if(tmpName) tmpName.text = userRanking.name;
        if(tmpTime) tmpTime.text = $"#{userRanking.id}";
        if(tmpScore) tmpScore.text = userRanking.score.ToString();
    }
}