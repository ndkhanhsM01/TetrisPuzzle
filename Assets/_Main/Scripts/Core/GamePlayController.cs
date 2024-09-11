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

    [Header("Configures")]
    [Min(1f)][SerializeField] private float fallingFastRate = 2f;
    [SerializeField] private float normalFallingInterval = 0.3f;
    [Header("Repeat input rate")]
    [Min(0f)][SerializeField] private float horizontalRate = 0.2f;
    [Min(0f)][SerializeField] private float rotateRate = 0.2f;

    private Shape activeShape;
    private float fallingInterval = 0f;
    private float fallingTimer = 0f;
    private float timerHorizontal = 0f;
    private float timerRotate = 0f;

    private bool isStopFalling = false;
    private bool isListenInput = true;

    public bool IsRotateClockwise { get; set; }
    public bool IsGamePause { get; private set; } = false;
    public Board Board => board;

    private void Start()
    {
        fallingInterval = normalFallingInterval;
        isListenInput = true;
        if (!activeShape)
        {
            activeShape = spawner.SpawnShape();
        }
    }
    private void Update()
    {
        if (IsGamePause) return;
        timerHorizontal -= Time.deltaTime;
        timerRotate -= Time.deltaTime;
        if (isListenInput)
        {
#if UNITY_EDITOR
            HandleInputOnPC();
#elif PLATFORM_ANDROID
        HandleInputOnMobile();
#endif
        }

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
        EventsCenterInGame.OnGameEnd?.Invoke(EndGameStatus.Lose);
        Debug.LogWarning("Game lose");
    }

#endif
    public void PauseGame()
    {
        EventsCenterInGame.OnPauseGame?.Invoke();
        IsGamePause = true;
    }

    public void UnpauseGame()
    {
        EventsCenterInGame.OnUnpauseGame?.Invoke();
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

        isStopFalling = true;

        if(board.IsOverLimit(activeShape))
        {
            EventsCenterInGame.OnGameEnd?.Invoke(EndGameStatus.Lose);
            Debug.LogWarning("Game lose");
            return;
        }

        await board.CheckClearAllRows();
        isStopFalling = false;
        activeShape = spawner.SpawnShape();
    }
    #endregion

    #region Handle Input

    private void HandleInputOnPC()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            DetectHoldFallingFast(true);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            DetectHoldFallingFast(false);


        if (timerHorizontal <= 0f && (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            timerHorizontal = horizontalRate;
            OnMoveRight();
        }
        else if (timerHorizontal <= 0f && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            timerHorizontal = horizontalRate;
            OnMoveLeft();
        }
        else if (timerRotate <= 0f && (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)))
        {
            timerRotate = rotateRate;
            OnRotate();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnDropImmidiate();
        }
    }

    private void HandleInputOnMobile()
    {

    }
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
        isStopFalling = true;
        while (isStopFalling)
        {
            activeShape.MoveDown();
            await CheckLandActiveShape();
            await UniTask.WaitForSeconds(0.001f);
        }

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
