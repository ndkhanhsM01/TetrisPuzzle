using MLib;
using System;
using System.Collections.Generic;
using System.Text;

public class StartAudioDetector: MSingleton<StartAudioDetector>
{
    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
        
    }

    private void OnSceneLoaded()
    {
        if(MAudioManager.Instance)
            MAudioManager.Instance.PlayMusic(MSoundType.Background);

        EventsCenter.OnSceneLoaded -= OnSceneLoaded;
    }
}
