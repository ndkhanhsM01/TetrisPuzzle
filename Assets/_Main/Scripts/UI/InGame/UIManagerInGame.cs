

using MLib;

public class UIManagerInGame: MUIManager
{
    private void OnEnable()
    {
        EventsCenterInGame.OnGameEnd += OnEndGame;
    }

    private void OnDisable()
    {
        EventsCenterInGame.OnGameEnd -= OnEndGame;
    }
    private void OnEndGame(EndGameStatus status)
    {
        switch (status)
        {
            case EndGameStatus.Win: break;
            case EndGameStatus.Lose: this.ShowPanel<PanelGameOver>(); break;
        }
    }
}