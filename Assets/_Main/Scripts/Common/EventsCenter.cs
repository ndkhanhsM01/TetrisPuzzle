

using System;

public static class EventsCenter
{
    /// <summary>
    /// Will be called when loaded new scene
    /// </summary>
    public static Action OnSceneLoaded;

    /// <summary>
    /// Will be called when coins amount were be changed
    /// </summary>
    public static Action<int> OnCoinChanged;

    /// <summary>
    /// Will be called when user name was be changed
    /// </summary>
    public static Action<string> OnUserNameChanged;
}