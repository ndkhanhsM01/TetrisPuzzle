

using System;

public static class EventsCenterInGame
{
    public static Action<EndGameStatus> OnGameEnd;
    public static Action OnPauseGame;
    public static Action OnUnpauseGame;
}