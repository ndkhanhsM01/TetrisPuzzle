using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using MLib;
using System;

public enum EndGameStatus { Lose, Win}

public class GamePlayController : MSingleton<GamePlayController>
{
    [Header("References")]
    [SerializeField] private Board board;
    [SerializeField] private ShapeSpawner spawner;
    [SerializeField] private GhostShape ghostShape;
    [SerializeField] private PlayerInputDetector inputDetector;

    [Header("Configures")]
    [Min(1f)][SerializeField] private float fallingFastRate = 2f;
    [SerializeField] private float normalFallingInterval = 0.3f;

    private Shape activeShape;
    private float fallingInterval = 0f;
    private float fallingTimer = 0f;
    private float timerHorizontal = 0f;
    private float timerRotate = 0f;

    private bool isStopFalling = false;
    private bool isListenInput = true;

    public bool IsRotateClockwise { get; set; }
    public bool IsGamePause { get; private set; } = false;
    public bool IsEndGame { get; private set; } = false;
    public Board Board => board;

    public Action<EndGameStatus> OnGameEnd;
    public Action OnPauseGame;
    public Action OnUnpauseGame;

    private void Start()
    {
        fallingInterval = normalFallingInterval;
        isListenInput = true;
        inputDetector.IsActive = true;

    }

    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += OnGameReady;

        inputDetector.OnMoveRight += OnMoveRight;
        inputDetector.OnMoveLeft += OnMoveLeft;
        inputDetector.OnDrop += OnDropImmidiate;
        inputDetector.OnRotate += OnRotate;
        inputDetector.OnDetectFallingFast += DetectHoldFallingFast;
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= OnGameReady;

        inputDetector.OnMoveRight -= OnMoveRight;
        inputDetector.OnMoveLeft -= OnMoveLeft;
        inputDetector.OnDrop -= OnDropImmidiate;
        inputDetector.OnRotate -= OnRotate;
        inputDetector.OnDetectFallingFast -= DetectHoldFallingFast;
    }

    private void Update()
    {
        if (IsGamePause || IsEndGame) return;
        if(!isStopFalling) 
            AutoFallActiveShape();
    }

    private void LateUpdate()
    {
        if(isStopFalling) return;
        if(ghostShape && activeShape)
        {
            ghostShape.Draw(activeShape);
        }
    }

#if UNITY_EDITOR
    [MButton]
    private void CheatGameOver()
    {
        OnGameEnd?.Invoke(EndGameStatus.Lose);
        IsEndGame = true;
        Debug.LogWarning("Game lose");
    }

#endif

    private void OnGameReady()
    {
        if (!activeShape)
        {
            activeShape = spawner.SpawnShape();
        }
    }
    public void PauseGame()
    {
        OnPauseGame?.Invoke();
        IsGamePause = true;
    }

    public void UnpauseGame()
    {
        OnUnpauseGame?.Invoke();
        IsGamePause = false;
    }

    #region Handle active shape
    private async void AutoFallActiveShape()
    {
        if (activeShape && fallingTimer <= 0f)
        {
            fallingTimer = fallingInterval;

            activeShape.MoveDown();
        }

        fallingTimer -= Time.deltaTime;
        await CheckLandActiveShape();
    }
    private async UniTask CheckLandActiveShape()
    {
        if (!activeShape) return;
        if (board.IsValidPosition(activeShape)) return;

        activeShape.MoveUp();
        board.StoreShapeInGrid(activeShape);
        ghostShape.DisableGhost();

        inputDetector.ResetTouch();
        isStopFalling = true;

        if(board.IsOverLimit(activeShape))
        {
            OnGameEnd?.Invoke(EndGameStatus.Lose);
            IsEndGame = true;
            Debug.LogWarning("Game lose");
            return;
        }

        await board.CheckClearAllRows();
        isStopFalling = false;
        activeShape = spawner.SpawnShape();
    }
    #endregion

    #region Handle Input

    private void DetectHoldFallingFast(bool status)
    {
        if (status)
        {
            fallingTimer = 0f;
            fallingInterval = normalFallingInterval / fallingFastRate;
        }
        else
        {
            fallingInterval = normalFallingInterval;
        }
    }
    private async void OnDropImmidiate()
    {
        isListenInput = false;
        inputDetector.IsActive = false;
        isStopFalling = true;
        if(activeShape) activeShape.EnableTrail();
        await UniTask.WaitForSeconds(0.25f);
        while (isStopFalling && activeShape && !IsEndGame)
        {
            activeShape.MoveDown();
            await CheckLandActiveShape();
        }
        inputDetector.IsActive = true;
        isListenInput = true;
    }
    private void OnMoveRight()
    {
        if (!activeShape) return;
        activeShape.MoveRight();

        if (!board.IsValidPosition(activeShape))
            activeShape.MoveLeft();
    }
    private void OnMoveLeft()
    {
        if (!activeShape) return;
        activeShape.MoveLeft();

        if(!board.IsValidPosition(activeShape))
            activeShape.MoveRight();
    }
    private void OnRotate()
    {
        if (!activeShape) return;

        if (IsRotateClockwise)
        {
            activeShape.RotateRight();

            if (!board.IsValidPosition(activeShape))
                activeShape.RotateLeft();
        }
        else
        {
            activeShape.RotateLeft();

            if (!board.IsValidPosition(activeShape))
                activeShape.RotateRight();
        }

    }
    #endregion
}
