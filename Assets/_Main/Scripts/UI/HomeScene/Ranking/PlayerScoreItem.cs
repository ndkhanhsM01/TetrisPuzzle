

using MLib;
using TMPro;
using UnityEngine;

public class PlayerScoreItem: MonoBehaviour
{
    [SerializeField] private TMP_Text tmpIndex;
    [SerializeField] private TMP_Text tmpName;
    [SerializeField] private TMP_Text tmpTime;
    [SerializeField] private TMP_Text tmpScore;

    private UserRanking playerRanking;
    private LocalData localData => DataManager.Instance.LocalData;
    private void OnEnable()
    {
        EventsCenter.OnUserNameChanged += OnNameChanged;
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EventsCenter.OnUserNameChanged -= OnNameChanged;
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        playerRanking = UserRanking.CreateNew(localData.userName, localData.highScore);
        playerRanking.index = Random.Range(15, 40);
    }

    private void OnNameChanged(string newName)
    {
        playerRanking.name = newName;
        tmpName.text = newName;
    }

    public void Setup()
    {
        if (tmpIndex) tmpIndex.text = (playerRanking.index + 1).ToString();
        if (tmpName) tmpName.text = playerRanking.name;
        if (tmpTime) tmpTime.text = playerRanking.GetTimeFormat_1();
        if (tmpScore) tmpScore.text = playerRanking.score.ToString();
    }
}