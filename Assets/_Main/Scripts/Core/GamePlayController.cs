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

    [Header("Configures")]
    [Min(1f)][SerializeField] private float dropFastRate = 2f;
    [SerializeField] private float normalDropInterval = 0.3f;
    [Header("Repeat input rate")]
    [Min(0f)][SerializeField] private float horizontalRate = 0.2f;
    [Min(0f)][SerializeField] private float rotateRate = 0.2f;

    private Shape activeShape;
    private float dropInterval = 0f;
    private float dropTimer = 0f;
    private float timerHorizontal = 0f;
    private float timerRotate = 0f;

    private bool isStop = false;

    public Action<EndGameStatus> OnGameEnd;
    private void Start()
    {
        dropInterval = normalDropInterval;
        if(!activeShape)
        {
            activeShape = spawner.SpawnShape();
        }
    }
    private async void Update()
    {
        timerHorizontal -= Time.deltaTime;
        timerRotate -= Time.deltaTime;
#if UNITY_EDITOR
        HandleInputOnPC();
#elif PLATFORM_ANDROID
        HandleInputOnMobile();
#endif

        if (isStop) return;
        AutoDropActiveShape();
        await CheckLandActiveShape();
    }

    #region Handle active shape
    private void AutoDropActiveShape()
    {
        if (activeShape && dropTimer <= 0f)
        {
            dropTimer = dropInterval;

            activeShape.MoveDown();
        }

        dropTimer -= Time.deltaTime;
    }
    private async UniTask CheckLandActiveShape()
    {
        if (!activeShape) return;
        if (board.IsValidPosition(activeShape)) return;

        activeShape.MoveUp();
        board.StoreShapeInGrid(activeShape);

        isStop = true;

        if(board.IsOverLimit(activeShape))
        {
            OnGameEnd?.Invoke(EndGameStatus.Lose);
            Debug.LogWarning("Game lose");
            return;
        }

        await board.CheckClearAllRows();
        isStop = false;
        activeShape = spawner.SpawnShape();
    }
    #endregion

    #region Handle Input

    private void HandleInputOnPC()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            DetectHoldDropFast(true);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            DetectHoldDropFast(false);


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
        else if (timerRotate <= 0f && (Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            timerRotate = rotateRate;
            OnRotate();
        }
    }

    private void HandleInputOnMobile()
    {

    }
    private void DetectHoldDropFast(bool status)
    {
        if (status)
        {
            dropTimer = 0f;
            dropInterval = normalDropInterval / dropFastRate;
        }
        else
        {
            dropInterval = normalDropInterval;
        }
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
        activeShape.RotateRight();

        if(!board.IsValidPosition(activeShape))
            activeShape.RotateLeft();
    }
    #endregion
}
