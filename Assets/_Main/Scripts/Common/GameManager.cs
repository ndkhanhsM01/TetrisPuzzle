
using UnityEngine;
using MLib;

public class GameManager: MSingleton<GameManager>
{
    private void OnEnable()
    {
        DataManager.OnLoadLocalSuccess += OnLocalDataLoaded;
    }

    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= OnLocalDataLoaded;
    }

    private void OnLocalDataLoaded(LocalData data)
    {
        
    }
}