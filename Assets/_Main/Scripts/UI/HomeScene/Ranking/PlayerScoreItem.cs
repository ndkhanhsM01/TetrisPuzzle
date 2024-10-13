

using MLib;
using TMPro;
using UnityEngine;

public class PlayerScoreItem: MonoBehaviour
{
    [SerializeField] private FakeDataUsers fakeDataUsers;

    [Header("References")]
    [SerializeField] private TMP_Text tmpIndex;
    [SerializeField] private TMP_Text tmpName;
    [SerializeField] private TMP_Text tmpTime;
    [SerializeField] private TMP_Text tmpScore;

    private UserRanking playerRanking;
    private void OnEnable()
    {
        EventsCenter.OnUserNameChanged += OnNameChanged;
    }

    private void OnDisable()
    {
        EventsCenter.OnUserNameChanged -= OnNameChanged;
    }
    private void OnNameChanged(string newName)
    {
        playerRanking.name = newName;
        tmpName.text = newName;
    }

    public void Setup(int index, UserRanking playerRanking)
    {
        if (playerRanking == null) return;

        this.playerRanking = playerRanking;
        if (tmpIndex)
        {
            string indexDisplay = playerRanking.score > 0
                                ? (index + 1).ToString() 
                                : "??"; 
            tmpIndex.text = indexDisplay;
        }

        if (tmpName) tmpName.text = playerRanking.name;
        if (tmpTime) tmpTime.text = $"#{playerRanking.id}";
        if (tmpScore) tmpScore.text = playerRanking.score.ToString();
    }
}